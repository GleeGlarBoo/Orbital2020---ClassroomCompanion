
// No need anything as we are not inheriting from MonoBehaviour - dont have / cant use its start() and update() function
// Just a static class, where everything inside this GameManager class must be static
// not going to attach class to any game object since it is not a script
// Global information, global memory, only one instance is allocated for the game, accessible from any script (public)
public static class GameManagerTapTap
{
    // static attribute - unique and its memory will be there when we run the game, accessible from any script
    // access to these information using: GameManager.<name of attribute>
    public static float score = 0;
    public static int coinsEarned = 0;    
}

// will eventually display these information onto the screen