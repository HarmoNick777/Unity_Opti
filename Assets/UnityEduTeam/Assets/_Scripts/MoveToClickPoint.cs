using UnityEngine;
using UnityEngine.AI;

public class MoveToClickPoint : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    private Animator animator;

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100))
            {
                // si le click souris à donné une position acceptable on met à jour la destination de l'agent
                // après avoir vérifier que l'on a bien un navmeshagent sur le gameobject pour éviter une nullreferenceexeption
                // on oublie pas de prévenir 'intégrateur pour ses tests
                if (navMeshAgent != null)
                {
                    navMeshAgent.destination = hit.point;
                    navMeshAgent.isStopped = false;
                    Debug.Log("Player destination has been changed !");
                }
            }
        }

        //on vérifie si le player est arrivé à destination
        if (Vector3.Distance(transform.position, navMeshAgent.destination) < 0.1f && navMeshAgent.velocity.magnitude > 0.01f)
        {
            // toujours la vérif de la présence du component pour éviter la nullreference
            if (navMeshAgent != null)
            {
                navMeshAgent.isStopped = true;
                Debug.Log("Player reached their destination!");
            }
        }

        //on met à jour l'animation en fonction de la vitesse de l'agent
        //après avoir vérifier que le component est bien là pour éviter la nullreference
        if (navMeshAgent != null && navMeshAgent.velocity.magnitude > .1f)
        {
            animator.SetBool("running", true);
            Debug.Log("Starts running"); // était Starts walking alors que le player court toujours et ne marche jamais
        }
        else
        {
            animator.SetBool("running", false);
            //Debug.Log("Starts running"); // N'a aucun sens de dire qu'il court, puisque cette condition s'applique même quand le player est arrêté
        }

    }
}

