using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace UpsideDown.Environment
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private int damage;
        [SerializeField] private float damageRate;
        [SerializeField] private float speed;
        public float health;
        private float _speed;
        private float _health;
        private int _damage;
        private NavMeshAgent _navMeshAgent;
        private readonly Vector3 _coreDestination = Vector3.zero;
        private NavMeshPath _path;
        private Coroutine _recalculateCoroutine;

        private void Start()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _navMeshAgent.speed = _speed;
            _damage = damage;
            _path = new NavMeshPath();
            Recalculate();
        }

        private void OnEnable()
        {
            GridManager.OnNavigationUpdate += Recalculate;
        }

        private void OnDisable()
        {
            GridManager.OnNavigationUpdate -= Recalculate;
        }

        private void OnDestroy()
        {
            WaveManager.Instance.EnemyDestroyed(gameObject);
        }

        public void BuffSpeed(float dividedBy)
        {
            _speed = speed / dividedBy;
        }

        public void BuffDamage(float dividedBy)
        {
            _damage = (int)Math.Round(damage / dividedBy);
        }

        public void BuffHealth(float dividedBy)
        {
            _health = health / dividedBy;
        }
        
        public void TakeDamage(int damage)
        {
            _health -= damage;
            if (_health <= 0)
            {
                StopCoroutine(_recalculateCoroutine);
                Destroy(gameObject);
            }
        }

        private void Recalculate()
        {
            if (_recalculateCoroutine != null)
            {
                StopCoroutine(_recalculateCoroutine);
            }
            _recalculateCoroutine = StartCoroutine(RecalculateCoroutine());
        }

        private IEnumerator RecalculateCoroutine()
        {
            Debug.Log("Recalculating Route...");
            NavMesh.CalculatePath(transform.position, _coreDestination, NavMesh.AllAreas, _path);
            _navMeshAgent.SetPath(_path);
            while (_navMeshAgent.remainingDistance > 0) yield return null;
            Debug.Log("Arrived");
            if (transform.position is { x: 0, z: 0 })
            {
                GridManager.Instance.centreGrid.TakeDamage(_damage / 2);
                Destroy(gameObject);
            }
            transform.LookAt(new Vector3(_coreDestination.x, transform.position.y, _coreDestination.z));
            Debug.DrawRay(new Vector3(transform.position.x, 0.1f, transform.position.z), transform.forward, Color.red);
            if (Physics.Raycast(new Vector3(transform.position.x, 0.1f, transform.position.z), transform.forward, out RaycastHit hit, 0.3f))
            {
                Debug.Log(hit.collider.name);
                if (hit.collider.CompareTag("EdgeModel") || hit.collider.CompareTag("GridModel"))
                {
                    if (hit.collider.name == "Core(Clone)")
                    {
                        yield return new WaitForSeconds(0.5f);
                        Recalculate();
                        yield break;
                    }
                    
                    Grid grid = hit.transform.parent.GetComponent<Grid>();
                    while (true)
                    {
                        yield return new WaitForSeconds(damageRate);
                        if (grid.health <= 0) yield break;
                        grid.TakeDamage(_damage);
                        if (grid.health <= 0) yield break;
                        yield return null;
                    }
                }
            }
            else
            {
                yield return new WaitForSeconds(0.5f);
                Recalculate();
            }
        }
    }
}