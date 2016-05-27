namespace ServerMessage{
    public enum Types : short{
        INIT_GAME = 1000,
        CLIENT_LOADED = 1005,
        ALL_LOADED = 1010,
        MOVE_UNIT = 1015,
        SPAWN_UNIT = 1025,
        TELEPORT_UNIT = 1025,
        TURN_ENDED = 1030
    }
}