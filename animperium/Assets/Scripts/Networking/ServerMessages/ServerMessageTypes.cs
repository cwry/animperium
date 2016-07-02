namespace ServerMessage{
    public enum Types : short{
        INIT_GAME = 1000,
        CLIENT_LOADED = 1005,
        ALL_LOADED = 1010,
        SPAWN_UNIT = 1020,
        TURN_ENDED = 1030,
        UNIT_ABILITY = 1035,
    }
}