
public interface IPooledObject
{
    void OnObjectSpawn();
    void OnObjectDeactive();
    void SetDeactiveParent(DeactiveParents _deactiveParent);
    CustomBehaviour GetGameObject();
}