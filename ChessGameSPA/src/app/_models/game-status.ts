export enum GameStatus {
    NotYetBegun = 0,
    Inprogress = 1,
    CheckWhite = 2,
    CheckBlack = 3,
    // constraint: keep end of game status higher than this
    GAMEOVERMAN = 4,
    WinWhite = 5,
    WinBlack = 6,
    Draw = 7,
    DrawRequestWhite = 8,
    DrawRequestBlack = 9
}
