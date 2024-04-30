using System;
using UnityEngine;

public class Soleil : MonoBehaviour
{
    // Il faut s'enregistrer pour être notifié de la fin de la journée
    public event Action OnJourneeTerminee;


    // Le temps en minutes de jeu écoulées entre deux images
    public float DeltaMinutesEcoulees
    {
        get
        {
            float proportion;
            float progression = 1 - ProportionRestante;
            if (_ancienPourcentage > progression)
            {
                proportion = 1 - _ancienPourcentage + progression;
            }
            else
            {
                proportion = progression - _ancienPourcentage;
            }

            return proportion * dureeJournee;
        }
    }

    /// <summary>
    /// Proportion de la journée qui reste à écouler
    /// Valeur qui diminue, tout de suite après minuit on est à 1.0 et on diminue avec le temps jusqu'à 0.0
    /// </summary>
    public float ProportionRestante => dureeJourneeRestante / dureeJournee;

    /// <summary>
    /// Indique si on est présentement pendant la nuit (entre 21h et 5h am)
    /// </summary>
    public bool EstNuit => ProportionRestante >= progression21h || ProportionRestante <= progression5h;

    [Header("Rotation pour changer graduellement la direction des ombres")]
    [SerializeField] private Vector3 rotationDepart;
    [SerializeField] private Vector3 rotationFin;
    [Header("Couleurs projetées par le soleil")]
    [SerializeField] private Color morningColor;
    [SerializeField] private Color noonColor;
    [SerializeField] private Color nightColor;
    [SerializeField] private float vitesse = 10.0f; // 10 minutes par seconde

    private Light _light;
    private float _ancienPourcentage;    
    private float dureeJournee = ConstantesJeu.MINUTES_PAR_JOUR; // 24 heures
    private float dureeJourneeRestante;

    // Pour les différentes phases de la journée
    private const float progression21h = 21.0f / 24;
    private const float progression5h = 5.0f / 24;
    private const float progression8h = 8.0f / 24;
    private const float progression12h = 12.0f / 24;
    private const float progression18h = 18.0f / 24;

    void Awake()
    {
        _light = GetComponent<Light>();
        noonColor = _light.color;

        // On commence la première journée à 8:00
        dureeJourneeRestante = dureeJournee * (1 - progression8h); // Il reste 16 heures de jour
        _ancienPourcentage = 1 - ProportionRestante;
    }

    // Update is called once per frame
    void Update()
    {
        // Pour le calcul du nombre de minutes écoulées
        _ancienPourcentage = 1 - ProportionRestante;

        dureeJourneeRestante -= Time.deltaTime * vitesse;

        float progression = 1 - ProportionRestante;

        // De 22h00 à 4h00, il doit faire noir.
        // Pas assez noir à mon goût
        if (progression >= progression21h || progression <= progression5h)
        {
            _light.color = nightColor;
            _light.intensity = 0.0f;
        }
        else if (progression < progression8h)
        {
            float pourcentage = (progression - progression5h) / (progression8h - progression5h);

            _light.color = Utilitaires.vec2color(Vector3.Lerp(
                Utilitaires.color2vec(nightColor),
                Utilitaires.color2vec(morningColor),
                pourcentage));

            _light.intensity = Mathf.Lerp(0.0f, 1.0f, pourcentage);
        }
        else if (progression < progression12h)
        {
            float pourcentage = (progression - progression8h) / (progression12h - progression8h);

            _light.color = Utilitaires.vec2color(Vector3.Lerp(
                Utilitaires.color2vec(morningColor),
                Utilitaires.color2vec(noonColor),
                pourcentage));

            _light.intensity = 1.0f;
        }
        else if (progression < progression18h)
        {
            float pourcentage = (progression - progression12h) / (progression18h - progression12h);
            _light.color = noonColor;
            _light.intensity = Mathf.Lerp(1, 0.6f, pourcentage);
        }
        else
        {
            float pourcentage = (progression - progression18h) / (progression21h - progression18h);

            _light.color = Utilitaires.vec2color(Vector3.Lerp(
                Utilitaires.color2vec(noonColor),
                Utilitaires.color2vec(nightColor),
                pourcentage));

            _light.intensity = Mathf.Lerp(0.6f, 0.0f, pourcentage);
        }

        transform.rotation = Quaternion.Slerp(
            Quaternion.Euler(rotationDepart),
            Quaternion.Euler(rotationFin),
            progression
        );


        if (dureeJourneeRestante <= 0)
        {
            OnJourneeTerminee?.Invoke();
            dureeJourneeRestante = dureeJournee;
        }
    }
}