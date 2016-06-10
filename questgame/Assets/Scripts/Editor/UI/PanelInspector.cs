using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;

[CustomEditor(typeof(Panel), true)]
public class PanelInspector : Editor
{
	string newStateName = "newState";

	int newState = 0;

	private UnityEditorInternal.ReorderableList list;

	public override void OnInspectorGUI()
	{
		Panel thisPanel = target as Panel;

		if (!EditorApplication.isPlaying)
		{
			thisPanel.refreshPanelOnLoad = EditorGUILayout.Toggle("Refresh on Panel Start", thisPanel.refreshPanelOnLoad);

			int newIndex = EditorGUILayout.IntField("Starting State", thisPanel.CurrentState);

			if (newIndex != thisPanel.CurrentState)
			{
				thisPanel.SetDefaultState(newIndex);
			}

			EditorGUILayout.BeginHorizontal();

			newStateName = GUILayout.TextField(newStateName);

			if (GUILayout.Button("Add State " + newStateName, EditorStyles.miniButtonRight))
			{
				Animator animator = thisPanel.GetComponent<Animator>();

				AnimatorController controller = animator.runtimeAnimatorController as AnimatorController;

				if (controller == null)
				{

					string controllerPath = EditorUtility.SaveFilePanelInProject("New Animation Contoller", thisPanel.name + "controller", "controller", "message");

					controller = AnimatorController.CreateAnimatorControllerAtPath(controllerPath);

					AssetDatabase.ImportAsset(controllerPath);
				}

				GenerateTriggerableTransition(newStateName, controller);

				thisPanel.AddState(newStateName);

				AnimatorController.SetAnimatorController(animator, controller);

				thisPanel.SetDefaultState(0);
			}

			EditorGUILayout.EndHorizontal();

			EditorGUILayout.LabelField("Panel States");
			EditorGUI.indentLevel = 1;

			for (int i = 0; i < thisPanel.States.Count; i++)
			{
				EditorGUILayout.BeginHorizontal();

				EditorGUILayout.LabelField(i.ToString(),GUILayout.Width(25));

				EditorGUILayout.LabelField(thisPanel.States[i]);

				AnimationClip clip = null;

				if (GUILayout.Button("X", GUILayout.Width(25)))
				{
					AnimatorController controller = thisPanel.GetComponent<Animator>().runtimeAnimatorController as AnimatorController;
					if (controller != null)
					{
						for (int y = 0; y < controller.parameters.Length; y++)
						{
							if (controller.parameters[y].name == thisPanel.States[i])
							{
								controller.RemoveParameter(y);

								AnimatorStateMachine stateMachine = controller.layers[0].stateMachine;

								for (int z = 0; z < stateMachine.anyStateTransitions.Length; z++)
								{
									if (stateMachine.anyStateTransitions[z].destinationState.name == thisPanel.States[i])
									{
										clip = stateMachine.anyStateTransitions[z].destinationState.motion as AnimationClip;

										stateMachine.RemoveState(stateMachine.anyStateTransitions[z].destinationState);

										DestroyImmediate(clip, true);
										AssetDatabase.SaveAssets();
										break;
									}
								}

								break;
							}
						}
					}
					thisPanel.RemoveState(i);
				}

				EditorGUILayout.EndHorizontal();
			}

		}
		else
		{
			EditorGUILayout.BeginHorizontal();

			newState = Mathf.Clamp(EditorGUILayout.IntField(newState),0, thisPanel.States.Count - 1);

			if (GUILayout.Button("Switch to state " + thisPanel.States[newState], EditorStyles.miniButtonRight))
			{
				thisPanel.SetState(newState);
			}

			EditorGUILayout.EndHorizontal();

			EditorGUILayout.LabelField("Current State",thisPanel.CurrentState.ToString());
		}
	}

	private static AnimationClip GenerateTriggerableTransition(string name, AnimatorController controller)
	{
		// Create the clip
		var clip = AnimatorController.AllocateAnimatorClip(name);
		AssetDatabase.AddObjectToAsset(clip, controller);
		AssetDatabase.SaveAssets();

		// Create a state in the animatior controller for this clip
		var state = controller.AddMotion(clip);

		// Add a transition property
		controller.AddParameter(name, AnimatorControllerParameterType.Trigger);

		// Add an any state transition
		var stateMachine = controller.layers[0].stateMachine;
		var transition = stateMachine.AddAnyStateTransition(state);
		transition.AddCondition(AnimatorConditionMode.If, 0, name);
		return clip;
	}
}