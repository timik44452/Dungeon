using LiveWorld.Mobs.Core.BehaviourModels;

using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace LiveWorld.Mobs.Core
{
    public abstract class Mob : MonoBehaviour, ITarget, IActionSender
    {
        public bool IsInitialized { get; private set; }
        public MobConfiguration currentConfiguration { get; private set; }

        public int TypeIdentifier { get => !IsInitialized ? int.MinValue : currentModel.dna.GetHashCode(); }

        protected BehaviourModel currentModel;
        protected Dictionary<int, Memory> memories;
        protected Dictionary<byte, Action<ITarget>> commandsDictionary;
        
        #region other params
        protected float maximalViewDistance = 33F;
        protected float maximalSmellDistance = 33F;
        protected float maximalHearingDistance = 33F;
        protected float safe_distace = 10f;
        #endregion

        public void Initialize(MobConfiguration configuration, BehaviourModel model)
        {
            currentModel = model;
            currentConfiguration = configuration;
            memories = new Dictionary<int, Memory>();

            InitializeCommandDictionary();
            StartCoroutine(Monitoring());

            IsInitialized = true;
        }

        public void InvokeCommand(ITarget target, IEnumerable<byte> commands)
        {
            foreach (byte command in commands)
            {
                if (commandsDictionary.ContainsKey(command))
                    commandsDictionary[command].Invoke(target);
            }
        }

        public float GetLastEffect(ITarget target)
        {
            if (target.TypeIdentifier == TypeIdentifier)
            {
                return 1F;
            }
            else
            {
                return 0F;
            }
        }

        private void InitializeCommandDictionary()
        {
            commandsDictionary = new Dictionary<byte, Action<ITarget>>();

            commandsDictionary.Add(0, target => Await());
            commandsDictionary.Add(1, MoveTo);
            commandsDictionary.Add(2, RunAway);
            commandsDictionary.Add(3, Attack);
        }

        private IEnumerator Monitoring()
        {
            while (true)
            {
                float viewDistance = maximalViewDistance * currentConfiguration.eyePower;
                float smellDistance = maximalSmellDistance * currentConfiguration.smellPower;
                float hearingDistance = maximalHearingDistance * currentConfiguration.hearingPower;

                var viewedTargets = SceneUtility.Targets.FindAll(target =>
                {
                    if (TargetSystem.IsTargetEquals(this, target))
                    {
                        return false;
                    }

                    float angle = Vector3.Angle(transform.forward, target.transform.position - transform.position);
                    float distance = Vector3.Distance(transform.position, target.transform.position);

                    return
                        distance < viewDistance &&
                        angle < currentConfiguration.fieldOfView;
                });

                Memory currentMemory = null;
                ITarget currentTarget = null;

                foreach (var viewedTarget in viewedTargets)
                {
                    if (!memories.ContainsKey(viewedTarget.TypeIdentifier))
                    {
                        if (viewedTarget.TypeIdentifier == TypeIdentifier)
                            memories.Add(viewedTarget.TypeIdentifier, new Memory(currentModel.EmptyFeeling));
                        else
                            memories.Add(viewedTarget.TypeIdentifier, new Memory(currentModel.EmptyFeeling));
                    }

                    Memory memory = memories[viewedTarget.TypeIdentifier];

                    if (currentTarget == null || currentModel.MoreImportant(memory.feeling, currentMemory.feeling))
                    {
                        currentTarget = viewedTarget;
                        currentMemory = memory;
                    }
                }

                if (currentTarget != null)
                {
                    IActionSender actionSender = currentTarget.gameObject.GetComponent<IActionSender>();

                    if (actionSender != null)
                    {
                        currentMemory.lastActionType = actionSender.GetLastEffect(this);
                    }

                    InvokeCommand(currentTarget, currentModel.Reaction(currentMemory.feeling));
                    UpdateMemories(currentMemory);
                }

                yield return new WaitForSeconds(0.2F);
            }
        }

        private void UpdateMemories(Memory memory)
        {
            float s = 0.005F;

            if (memory.lastActionType == 0)
            {
                float old_fear = memory.feeling["fear"];
                float old_agression = memory.feeling["agression"];

                memory.feeling["fear"] -= s;
                memory.feeling["agression"] -= s;

                float fear_delta = old_fear - memory.feeling["fear"];
                float fear_agression = old_agression - memory.feeling["agression"];

                memory.feeling["interest"] += (fear_delta + fear_agression) - s;
            }
            else if(memory.lastActionType > 0)
            {
                memory.feeling["fear"] -= s;
                memory.feeling["agression"] -= s;
                memory.feeling["interest"] += s;
            }

            memory.lastActionType = 0;
        }

        #region Commands
        public virtual void Await()
        {

        }

        public virtual void Attack(ITarget target)
        {
            
        }

        public virtual void MoveTo(ITarget target)
        {
            transform.position += (transform.position - target.transform.position).normalized * Time.deltaTime;
        }

        public virtual void RunAway(ITarget target)
        {
            transform.position -= (transform.position - target.transform.position).normalized * Time.deltaTime;
        }
        #endregion
    }
}
