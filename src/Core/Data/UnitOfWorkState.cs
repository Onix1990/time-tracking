namespace Core.Data {
    public enum UnitOfWorkState {
        Ready = 0,
        Begun = 1,
        Committed = 2,
        Rollbacked = 3
    }
}