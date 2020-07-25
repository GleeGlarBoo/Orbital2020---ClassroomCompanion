
// FOR MATHHHHHHHHHHHH

// storing information for each question. Just an empty class for this
// Using object orientation programming. Describing the question object for our game 
// Whatever info associated to a question will be stored in here

using UnityEngine;
using UnityEngine.UI;

[System.Serializable]               // Can be saved and store info. Allows us to edit in inspector
public class Question
{
    public string question;
    public Sprite questionImage = null;
    public QuestionTopicMath topic;
    public int answer;
    public int numberOfOptions;

    public string option1;
    public string option2;
    public string option3;
    public string option4;

    public string explanation;      // for summary scene

}


public enum QuestionTopicMath { Primes_HCF_LCM, Integers, Approximation_Estimation, Algebra, LinearEquations, NumberPatterns, Percentage, Ratio_Rate_Speed, BasicGeometry, Triangles, Functions_LinearGraphs, Perimeter_Area, GeometricalConstructions, Volume_SurfaceArea, StatisticalDataHandling }
