public interface IPlayerControls
{
    void Forward();
    void Backward();
    void Jog();
    void Run();
    void TurnRight();
    void TurnLeft();
    void BasicAttack();
    void Kick();
    void StrongAttack();

    bool isRunning { get; set; }
    bool isJogging { get; set; }

    bool isStopped { get; set; }
}
