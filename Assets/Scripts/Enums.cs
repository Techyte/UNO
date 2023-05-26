namespace UNO.Enums
{
    public enum CardColour : ushort
    {
        NONE,
        RED,
        GREEN,
        BLUE,
        YELLOW
    }
        
    public enum CardType : ushort
    {
        NONE,
        ZERO,
        ONE,
        TWO,
        THREE,
        FOUR,
        FIVE,
        SIX,
        SEVEN,
        EIGHT,
        NINE,
        SKIP,
        REVERSE,
        DRAWTWO,
        WILD,
        DRAWFOUR,
        SHUFFLE
    }

    public enum TurnDirection : ushort
    {
        FORWARD,
        BACKWARD
    }
}