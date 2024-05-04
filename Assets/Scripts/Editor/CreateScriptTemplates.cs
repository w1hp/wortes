using UnityEditor;

public static class CreateScriptTemplates
{
	[MenuItem("Assets/Create/DOTS/Authoring Script", priority = 40)]
	public static void CreateAuthoring()
	{
		string templatePath = "Assets/Code/Editor/NewAuthoring.cs.txt";
		ProjectWindowUtil.CreateScriptAssetFromTemplateFile(templatePath, "NewAuthoring.cs");
	}
	[MenuItem("Assets/Create/DOTS/System Script", priority = 40)]
	public static void CreateSystem()
	{
		string templatePath = "Assets/Code/Editor/NewSystem.cs.txt";
		ProjectWindowUtil.CreateScriptAssetFromTemplateFile(templatePath, "NewSystem.cs");
	}
}