/*
 * A static method, field, property, or event is callable on a class even when no instance of the class has been created. ... 
 * Static means that the variable belongs to the class and not the object (in Unity). That means that only one instance of it will be created and will remain over any scope.
 */

using UnityEngine;

public static class GameManager 
{
    // FIELDS ARE RESET AND INITIALIZED IN UserInterface script

    public static int doggoLevel = 1;                 // default value = 1, adjusted elsewhere in scripts, initialized in UserInterface

    [Range(0, 1)]
    public static float doggoExperience = 0f;       // default value = 0, adjusted elsewhere in scripts, initialized in UserInterface

    [Range(0, 9999)]
    public static int money = 100;                  // default value = 100, adjusted elsewhere in scripts, initialized in UserInterface

    public static int score = 0;                    


    // Flag for any quiz that is finished and also the index of what quiz is finished. 
    // Set in Scoring script.
    // Used in PopUpMenu script - start() and FinishedQuiz()
    // public static int QuizIndex = 0;

}
