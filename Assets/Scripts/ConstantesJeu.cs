public static class ConstantesJeu
{
    // Ça représente la quantité d'énergie que le joueur perd à chaque minute
    // dans le jeu
    public const float COUT_MARCHER = 0.001f;
    public const float COUT_COURIR = 0.005f;
    public const float COUT_PLANTER = 0.002f;
    public const float COUT_CUEILLIR = 0.001f;
    public const float COUT_IMMOBILE = 0.0001f;

    // Ce que le joueur peut gagner en énergie
    public const float GAIN_ENERGIE_MANGER_OEUF = 0.25f;
    public const float GAIN_ENERGIE_MANGER_CHOU = 0.35f;
    public const float GAIN_ENERGIE_SOMMEIL = 0.005f;    // Le joueur gagne de l'énergie en dormant

    // Constantes relatives au temps dans une journée
    public const float MINUTES_PAR_JOUR = 1440.0f;
}
