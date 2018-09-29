using System;
using System.Collections.Generic;
using UnityEngine;

namespace LLtK
{
    public class King : MovingAgent {

        public Kingdom Kingdom;
        public GameObject MovingGameObject;
        public GameObject StayingGameObject;
        public GameObject CurrentGameObject;
        public List<Village> Path;
        public int CurrentDestination;
        public Village StayingVillage;

        public void Init(GameManager gameManager, Kingdom kingdom)
        {
            GameManager = gameManager;
            Kingdom = kingdom;
            Kingdom.AddPossessedAgent(this);
            GameManager.Kings.Add(this);
            CurrentDestination = 1;

            foreach (Agent agent in Kingdom.PossessedAgents)
            {
                if (agent.GetType() == typeof(Village))
                {
                    InstantiateStayingGameObject((Village) agent);

                    break;
                }
            }
        }

        public void InstantiateStayingGameObject(Village village)
        {
            if(CurrentGameObject != null) Destroy(CurrentGameObject);
            if (gameObject.GetComponent<SphereCollider>() != null) Destroy(gameObject.GetComponent<SphereCollider>());
            if (gameObject.GetComponent<Rigidbody>() != null) Destroy(gameObject.GetComponent<Rigidbody>());

            StayingVillage = village;

            gameObject.transform.position = village.transform.position;
            CurrentGameObject = Instantiate(StayingGameObject, gameObject.transform);
            CurrentGameObject.transform.position = new Vector3(village.transform.position.x, village.transform.position.y + 20, village.transform.position.z);

            KingBoost kingBoost = new KingBoost();
            kingBoost.Start(village);
            village.Controller.AddAgentBehaviour(kingBoost);
        }

        public void InstantiateMovingGameObject(Village source, Village destination)
        {
            if (CurrentGameObject != null) Destroy(CurrentGameObject);

            CurrentGameObject = Instantiate(MovingGameObject, transform);

            Vector3 vector = destination.transform.position - source.transform.position;
            vector = vector.normalized;

            double angle;
            if (vector[2] <= 0 && vector[0] <= 0)
            {
                angle = (Math.Acos(vector[0]) * 180 / Math.PI) + 180;
            }
            else if (vector[2] <= 0 && vector[0] > 0)
            {
                angle = (Math.Acos(vector[0]) * 180 / Math.PI) + 180;
            }
            else if (vector[2] > 0 && vector[0] <= 0)
            {
                angle = -(Math.Acos(vector[0]) * 180 / Math.PI) + 180;
            }
            else
            {
                angle = -(Math.Acos(vector[0]) * 180 / Math.PI) + 180;
            }

            gameObject.transform.position = source.transform.position + (source.GetComponent<SphereCollider>().radius) * vector;
            CurrentGameObject.transform.eulerAngles = new Vector3(0.0f, (float)angle, 0.0f);
        }

        void OnTriggerEnter(Collider other)
        {
            Village village = other.transform.GetComponent<Village>();
            if (village && village == Path[Path.Count - 1])
            {
                Village collidedVillage = other.gameObject.GetComponent<Village>();

                if (collidedVillage.Kingdom == Kingdom)
                {
                    Debug.Log("J'ai rencontré un village allié !");
                    Path.Clear();
                    StayingVillage = collidedVillage;
                    CurrentDestination = 1;
                    Controller.GetAgentBehaviour<GoTo>().Stop();

                    InstantiateStayingGameObject(collidedVillage);
                }
                else
                {
                    Remove();
                    Debug.Log("You loose !");
                }
            }
            else if (village && village == Path[CurrentDestination])
            {
                Village collidedVillage = other.gameObject.GetComponent<Village>();

                if (collidedVillage.Kingdom == Kingdom)
                {
                    Debug.Log("J'ai rencontré un village allié !");

                    CurrentDestination++;
                    Controller.GetAgentBehaviour<GoTo>().Stop();

                    Vector3 vector = Path[CurrentDestination].transform.position - Path[CurrentDestination - 1].transform.position;
                    vector = vector.normalized;

                    double angle;
                    if (vector[2] <= 0 && vector[0] <= 0)
                    {
                        angle = (Math.Acos(vector[0]) * 180 / Math.PI) + 180;
                    }
                    else if (vector[2] <= 0 && vector[0] > 0)
                    {
                        angle = (Math.Acos(vector[0]) * 180 / Math.PI) + 180;
                    }
                    else if (vector[2] > 0 && vector[0] <= 0)
                    {
                        angle = -(Math.Acos(vector[0]) * 180 / Math.PI) + 180;
                    }
                    else
                    {
                        angle = -(Math.Acos(vector[0]) * 180 / Math.PI) + 180;
                    }

                    transform.position = Path[CurrentDestination - 1].transform.position + Path[CurrentDestination - 1].GetComponent<SphereCollider>().radius * vector;
                    transform.eulerAngles = new Vector3(0.0f, (float)angle, 0.0f);

                    GoTo goTo = new GoTo();
                    goTo.Start(this, Path[CurrentDestination - 1], Path[CurrentDestination]);
                    Controller.AddAgentBehaviour(goTo);
                }
                else
                {
                    Remove();
                    Debug.Log("You loose");
                }
            }

            Army collidedArmy = other.transform.GetComponent<Army>();
            if (collidedArmy)
            {
                if (collidedArmy.Kingdom != Kingdom)
                {
                    Remove();
                    Debug.Log("You loose");
                }
            }
        }

        public void Remove()
        {
            if (StayingVillage != null)
            {
                StayingVillage.Controller.GetAgentBehaviour<KingBoost>().Stop();
            }

            if (CurrentGameObject != null) Destroy(CurrentGameObject);
            GameManager.Kings.Remove(this);
            Destroy(this);
        }

        public void Move(Village source, Village destination)
        {
            if (this == destination) return;

            Path = aStar(source, destination);
            InstantiateMovingGameObject(Path[0], Path[1]);

            StayingVillage.Controller.GetAgentBehaviour<KingBoost>().Stop();

            GoTo goTo = new GoTo(); ;
            goTo.Start(this, Path[0], Path[1]);
            Controller.AddAgentBehaviour(goTo);

            gameObject.AddComponent<SphereCollider>();
            gameObject.GetComponent<SphereCollider>().radius = 10;
            gameObject.GetComponent<SphereCollider>().center = new Vector3(0,5,0);
            gameObject.GetComponent<SphereCollider>().isTrigger = true;

            gameObject.AddComponent<Rigidbody>();
            gameObject.GetComponent<Rigidbody>().isKinematic = true;
            StayingVillage = null;
        }

        List<Village> aStar(Village start, Village goal)
        {
            List<Village> closedSet = new List<Village>();
            List<Village> openSet = new List<Village>();
            openSet.Add(start);

            Dictionary<Village, Village> cameFrom = new Dictionary<Village, Village>();
            Dictionary<Village, int> gScore = new Dictionary<Village, int>();
            Dictionary<Village, int> fScore = new Dictionary<Village, int>();

            foreach (Village village in GameManager.Villages)
            {
                gScore.Add(village, int.MaxValue);
                fScore.Add(village, int.MaxValue);
            }

            gScore[start] = 0;
            fScore[start] = heuristic_cost_estimate(start, goal);

            while (openSet.Count != 0)
            {
                Village current = null;
                int min = int.MaxValue;
                foreach (Village village in openSet)
                {
                    int temp = fScore[village];
                    if (min > temp)
                    {
                        current = village;
                        min = temp;
                    }
                }

                if (current == goal) return reconstruct_path(cameFrom, current);
                openSet.Remove(current);
                closedSet.Add(current);

                foreach (Village neighbour in current.Neighbours())
                {
                    if (closedSet.Contains(neighbour)) continue;
                    if (!openSet.Contains(neighbour)) openSet.Add(neighbour);
                    int tentativeGScore = gScore[current] + heuristic_cost_estimate(current, neighbour);
                    if (tentativeGScore >= gScore[neighbour]) continue;
                    cameFrom[neighbour] = current;
                    gScore[neighbour] = tentativeGScore;
                    fScore[neighbour] = gScore[neighbour] + heuristic_cost_estimate(neighbour, goal);
                }
            }
            return null;
        }

        int heuristic_cost_estimate(Village first, Village second)
        {
            int distance =
                (int)Vector3.Distance(first.gameObject.transform.position, second.gameObject.transform.position);
            if (second.Kingdom != Kingdom)
            {
                distance += 500;
            }
            return distance;
        }

        List<Village> reconstruct_path(Dictionary<Village, Village> cameFrom, Village current)
        {
            List<Village> path = new List<Village>();
            path.Add(current);
            while (cameFrom.ContainsKey(current))
            {
                current = cameFrom[current];
                path.Add(current);
            }
            path.Reverse(0, path.Count);
            return path;
        }
    }
}
