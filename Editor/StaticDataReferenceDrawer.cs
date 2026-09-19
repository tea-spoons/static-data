
namespace TeaSpoons.StaticData.Editor
{
    using TeaSpoons.PackageCore.Editor;
    using UnityEditor;
    using UnityEngine;

    [CustomPropertyDrawer(typeof(StaticDataReference<>))]
    public class StaticDataReferenceDrawer : PropertyDrawer
    {
        private StaticDataObject currentTarget;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            
            if (property.TryGetTargetObject<IStaticDataReference>(out var reference))
            {
                var idProperty = property.FindPropertyRelative(StaticDataReference<StaticDataObject>.PropertyNames.Id);

                if (!IdMatchesCurrentTarget(idProperty))
                {
                    currentTarget = reference.GetBaseTypeTarget();
                }

                var targetWasNull = currentTarget == null;
                var newTarget = (StaticDataObject)EditorGUI.ObjectField(position, label, currentTarget, reference.Type, false);
                if (newTarget != currentTarget)
                {
                    currentTarget = newTarget;

                    if (currentTarget != null)
                    {
                        idProperty.stringValue = currentTarget.Id;

                        var path = AssetDatabase.GetAssetPath(currentTarget);
                    }
                    else
                    {
                        // We only nullify the id properties if we switched from non-null to null target with the ObjectField.
                        // If currentTarget was null before, the asset might just be missing.
                        // Keeping the asset guid intact for that case means we rediscover an asset that had, for example, import issues.
                        if (!targetWasNull)
                        {
                            idProperty.stringValue = string.Empty;
                        }
                    }
                }

                // Fix incorrect id to match the asset
                if (!IdMatchesCurrentTarget(idProperty))
                {
                    idProperty.stringValue = currentTarget?.Id ?? string.Empty;
                }
            }

            EditorGUI.EndProperty();
        }

        private bool IdMatchesCurrentTarget(SerializedProperty idProperty)
        {
            if (currentTarget == null && string.IsNullOrEmpty(idProperty.stringValue))
            {
                return true;
            }

            if (currentTarget != null && !string.IsNullOrEmpty(idProperty.stringValue))
            {
                return idProperty.stringValue == currentTarget.Id;
            }

            return false;
        }
    }
}
