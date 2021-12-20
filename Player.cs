using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour, IRouteAgent
{
    [SerializeField]
    private float _speed;
    private Coroutine _coroutineStart;

    public Animator anim;


    private bool _allowToRollDice;
    public bool AllowedToRollDice
    {
        get
        {
            return _allowToRollDice;
        }
        set
        {
            _allowToRollDice = value;

            if (_allowToRollDice && _coroutineStart == null)
                _coroutineStart = StartCoroutine(WaitForClickAndRollDice());
        }
    }

    [field:SerializeField]
    public RollDiceScriptableEvent RollDiceScriptableEvent { get; set ; }

    public void MoveImmediately(Vector3 destination)
    {
        transform.position = destination;
    }

    public IEnumerator MoveTo(Vector3 destination)
    {

        anim.SetBool("isMoving", true);

        while (Vector3.Distance(transform.position, destination) > _speed * Time.deltaTime)
        {
            if (transform.position.x > destination.x)
                transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
            else
                transform.localRotation = Quaternion.Euler(new Vector3(0, 180, 0));

            transform.position = Vector3.MoveTowards(transform.position, destination, _speed * Time.deltaTime);
            yield return null;
        }

        anim.SetBool("isMoving", false);
    }

    private IEnumerator WaitForClickAndRollDice()
    {
        while (!Input.GetKeyDown(KeyCode.Space))
        {
            yield return null;
        }

        RollDiceScriptableEvent?.OnDiceRoll?.Invoke();
        _coroutineStart = null;
    }
}
