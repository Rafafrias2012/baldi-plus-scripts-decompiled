using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

// Token: 0x0200009A RID: 154
public class ValueReference
{
	// Token: 0x06000366 RID: 870 RVA: 0x00011D45 File Offset: 0x0000FF45
	public ValueReference(MemberInfo memberInfo, object referenceObject, object value, string pathToObject)
	{
		this.memberInfo = memberInfo;
		this.referenceComponent = referenceObject;
		this.value = value;
		this.pathToObject = pathToObject;
	}

	// Token: 0x06000367 RID: 871 RVA: 0x00011D6A File Offset: 0x0000FF6A
	public ValueReference(MethodInfo methodInfo, object referenceObject)
	{
		this.methodInfo = methodInfo;
		this.referenceComponent = referenceObject;
	}

	// Token: 0x06000368 RID: 872 RVA: 0x00011D80 File Offset: 0x0000FF80
	public void Resave()
	{
		this.SetValue(null, true, true);
	}

	// Token: 0x06000369 RID: 873 RVA: 0x00011D8B File Offset: 0x0000FF8B
	public void SetValue(bool gameToEditor)
	{
		this.SetValue(this.value, gameToEditor, false);
	}

	// Token: 0x0600036A RID: 874 RVA: 0x00011D9C File Offset: 0x0000FF9C
	private void SetValue(object value, bool gameToEditor, bool reset = false)
	{
		Debug.Log(string.Format("Setting... {0}.", string.Concat(new string[]
		{
			(this.referenceComponent as Component).gameObject.name,
			"/",
			this.referenceComponent.GetType().Name,
			"/",
			this.pathToObject,
			this.memberInfo.Name
		})));
		if (gameToEditor)
		{
			string text = GameObjectUtilties.BuildPathFromGameObject((this.referenceComponent as Component).gameObject, "");
			GameObject gameObject = GameObject.Find(text);
			if (!(gameObject != null))
			{
				Debug.LogWarning(string.Format("Cannot access GameObject at {0}, is it disabled?", text));
				this.referenceComponent = null;
				return;
			}
			this.referenceComponent = gameObject.GetComponent(this.referenceComponent.GetType());
		}
		object reference = this.GetReference(this.referenceComponent, this.pathToObject);
		if (this.memberInfo is PropertyInfo)
		{
			PropertyInfo propertyInfo = this.memberInfo as PropertyInfo;
			if (value != null)
			{
				if (propertyInfo.PropertyType == typeof(GameObject))
				{
					string text2 = value as string;
					if (text2 == null)
					{
						Debug.LogWarning(string.Format("Cannot access GameObject at {0}, is it disabled?", this.memberInfo.Name));
						return;
					}
					value = GameObject.Find(text2);
				}
				else if (propertyInfo.PropertyType.IsSubclassOf(typeof(Component)) || propertyInfo.PropertyType == typeof(Component))
				{
					string text3 = (value as string) ?? "NO PATH";
					int num = text3.LastIndexOf('/');
					if (num < 0)
					{
						Debug.LogWarning(string.Format("Could not find Component {0} at {1}", this.memberInfo.Name, text3));
						return;
					}
					Debug.Log(text3.Substring(0, num));
					Debug.Log(text3.Substring(num + 1));
					GameObject gameObject2 = GameObject.Find(text3.Substring(0, num));
					if (!(gameObject2 != null))
					{
						Debug.LogWarning(string.Format("Cannot access GameObject at {0}, is it disabled?", this.memberInfo.Name));
						return;
					}
					value = gameObject2.GetComponent(text3.Substring(num + 1));
				}
				else if (propertyInfo.PropertyType.IsSubclassOf(typeof(Object)))
				{
					if (!(value is string))
					{
						value = null;
					}
					else
					{
						IEnumerable<Object> source = from o in Resources.FindObjectsOfTypeAll(propertyInfo.PropertyType)
						where o.name == value as string
						select o;
						if (source.Count<Object>() == 1)
						{
							value = source.First<Object>();
						}
						else
						{
							if (source.Count<Object>() == 0)
							{
								Debug.LogError("Property is Object but couldn't find it by type!");
								string str = "name: ";
								object obj = value;
								Debug.Log(str + ((obj != null) ? obj.ToString() : null));
								Debug.Log("type: " + propertyInfo.PropertyType.ToString());
								return;
							}
							Debug.LogError("Amibiguous Object!");
							string str2 = "name: ";
							object obj2 = value;
							Debug.Log(str2 + ((obj2 != null) ? obj2.ToString() : null));
							Debug.Log("type: " + propertyInfo.PropertyType.ToString());
							return;
						}
					}
				}
				if (value != null)
				{
					propertyInfo.SetValue(reference, value, null);
					Debug.Log(string.Format("Set as {0}", value.ToString()));
					return;
				}
				propertyInfo.SetValue(reference, null, null);
				Debug.Log(string.Format("Set as null", Array.Empty<object>()));
				return;
			}
			else
			{
				if (reset)
				{
					value = propertyInfo.GetValue(reference, null);
					Debug.Log(string.Format("Saved as {0}", value.ToString()));
					return;
				}
				propertyInfo.SetValue(reference, null, null);
				Debug.Log(string.Format("Set as null", Array.Empty<object>()));
				return;
			}
		}
		else
		{
			if (!(this.memberInfo is FieldInfo))
			{
				Debug.LogError("No reference is attached to this ValueReference");
				return;
			}
			FieldInfo fieldInfo = this.memberInfo as FieldInfo;
			if (value != null)
			{
				if (fieldInfo.FieldType == typeof(GameObject))
				{
					string text4 = value as string;
					if (text4 == null)
					{
						Debug.LogWarning(string.Format("Cannot access GameObject at {0}, is it disabled?", this.memberInfo.Name));
						return;
					}
					value = GameObject.Find(text4);
				}
				else if (fieldInfo.FieldType.IsSubclassOf(typeof(Component)) || fieldInfo.FieldType == typeof(Component))
				{
					string text5 = value as string;
					int num2 = text5.LastIndexOf('/');
					if (num2 < 0)
					{
						Debug.LogWarning(string.Format("Could not find Component {0} at {1}", this.memberInfo.Name, text5));
						return;
					}
					Debug.Log(text5.Substring(0, num2));
					Debug.Log(text5.Substring(num2 + 1));
					GameObject gameObject3 = GameObject.Find(text5.Substring(0, num2));
					if (!(gameObject3 != null))
					{
						Debug.LogWarning(string.Format("Cannot access GameObject at {0}, is it disabled?", this.memberInfo.Name));
						return;
					}
					value = gameObject3.GetComponent(text5.Substring(num2 + 1));
				}
				else if (fieldInfo.FieldType.IsSubclassOf(typeof(Object)))
				{
					if (!(value is string))
					{
						value = null;
					}
					else
					{
						IEnumerable<Object> source2 = from o in Resources.FindObjectsOfTypeAll(fieldInfo.FieldType)
						where o.name == value as string
						select o;
						if (source2.Count<Object>() == 1)
						{
							value = source2.First<Object>();
						}
						else
						{
							if (source2.Count<Object>() == 0)
							{
								Debug.LogError("Field is Object but couldn't find it by type!");
								string str3 = "name: ";
								object obj3 = value;
								Debug.Log(str3 + ((obj3 != null) ? obj3.ToString() : null));
								Debug.Log("type: " + fieldInfo.FieldType.ToString());
								return;
							}
							Debug.LogError("Amibiguous Object!");
							string str4 = "name: ";
							object obj4 = value;
							Debug.Log(str4 + ((obj4 != null) ? obj4.ToString() : null));
							Debug.Log("type: " + fieldInfo.FieldType.ToString());
							return;
						}
					}
				}
				if (value != null)
				{
					fieldInfo.SetValue(reference, value);
					Debug.Log(string.Format("Set as {0}", value.ToString()));
					return;
				}
				fieldInfo.SetValue(reference, null);
				Debug.Log(string.Format("Set as null", Array.Empty<object>()));
				return;
			}
			else
			{
				if (reset)
				{
					value = fieldInfo.GetValue(reference);
					Debug.Log(string.Format("Saved as {0}", value.ToString()));
					return;
				}
				fieldInfo.SetValue(reference, null);
				Debug.Log(string.Format("Set as null", Array.Empty<object>()));
				return;
			}
		}
	}

	// Token: 0x0600036B RID: 875 RVA: 0x000124A4 File Offset: 0x000106A4
	private object GetReference(object o, string path)
	{
		object obj = o;
		foreach (string text in path.Split(new char[]
		{
			'/'
		}))
		{
			if (text.Length <= 0)
			{
				break;
			}
			obj = obj.GetType().GetField(text).GetValue(obj);
		}
		return obj;
	}

	// Token: 0x040003A5 RID: 933
	public MemberInfo memberInfo;

	// Token: 0x040003A6 RID: 934
	public MethodInfo methodInfo;

	// Token: 0x040003A7 RID: 935
	public object referenceComponent;

	// Token: 0x040003A8 RID: 936
	public string pathToObject;

	// Token: 0x040003A9 RID: 937
	public object value;
}
