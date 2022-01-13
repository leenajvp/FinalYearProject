public interface IPlayerControls
{
    void Forward();
    void Backward();
    void TurnRight();
    void TurnLeft();
    void WeaponAttack();
    void Kick();
    void ChangeGun();

    bool isRunning { get; set; }
    bool isCrawling { get; set; }

    bool isStopped { get; set; }
}
