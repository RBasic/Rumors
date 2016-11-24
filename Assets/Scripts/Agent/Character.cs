using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour
{

    //ATTRIBUTS
    //float doute = 0; //Croyance de la rumeur 0 = croit pas, 1 = croit, 0.5 = a moitie
    //bool auCourant = false; //au courant de la rumeur
    //bool enParle = false; //est en train de parler, interaction
    //int cptEntendu = 0; // nombre de fois ou on lui parle de la rumeur
    float avisGroupe = 0; //avis du groupe sur la rumeur
    //bool aime = false; //aime parler de la rumeur

    //ROLES
    //bool investigateur = false; //si doute > 0.5 alors +0.1 sinon -0.1
    //bool interprete = false; //si interaction avec investigateur ayant doute opposé alors + ou - 0.1
    //bool leader = false; //meneur du groupe + ou -0.2 -> + ou - 0.1 aux personnes du groupe


   // bool apotre = false; //doute +0.1
    bool recupereur = false; //pas de doute, doute +0.1
    //Meme chose nan ? a part si doute apotre == 0 alors arrete de propager


   // bool relaisPassif = false;  //pas de doute, si doute personne > 0.5 alors +0.1 
                                //si doute personne < 0.5 alors -0.1 
                                //si doute personne ==0.5 alors +0.0
    //bool resistant = false; //doute -0.1

}
