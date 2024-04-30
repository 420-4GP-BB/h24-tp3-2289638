public interface IActionnable
{
    EtatJoueur EtatAUtiliser(ComportementJoueur sujet);
    bool Permis(ComportementJoueur sujet);
}