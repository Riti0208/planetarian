using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

using System;
using System.Linq;

using UnityEditor;

public class loadStarDataTxt : MonoBehaviour {

	static public loadStarDataTxt Instance;
	public List<starParamsGlue> starParamsList;
	static int GenerateStarCnt = 0;
	static starParamList splItem;

	[MenuItem ("Assets/CreateStarData")]
	static void loadStarData () {
		TextAsset csv = Resources.Load("starData") as TextAsset;
		StringReader reader = new StringReader(csv.text);
		reader.ReadLine();
		while (reader.Peek() > -1) {
			registStar (reader);
		}
	}
	[MenuItem ("Assets/CreateStarDataSingle")]
	static void loadStarDataForSingle(){
		TextAsset csv = Resources.Load("starData") as TextAsset;
		StringReader reader = new StringReader(csv.text);
		reader.ReadLine();
		splItem = ScriptableObject.CreateInstance<starParamList> ();
		splItem.starParam = new List<starParamsGlue> ();
		while (reader.Peek() > -1) {
			registStarForSingle (reader);
		}
		string path = AssetDatabase.GenerateUniqueAssetPath("Assets/Resources/starDataScriptableObject/starParams.asset");

		AssetDatabase.CreateAsset(splItem, path);
		AssetDatabase.SaveAssets();
		AssetDatabase.Refresh();
		Debug.Log (splItem.name + " Output Compleate!!");
	}

	[MenuItem ("Assets/CreateStarObjects")]
	static void CreateStarObjects(){
		GameObject 	skyObj = 				GameObject.Find ("blackSky");
		if (skyObj == null) {
			skyObj = Instantiate (Resources.Load<GameObject>("Prefabs/blackSky"));
		}
		GameObject 	starPrefab = 			Resources.Load<GameObject> ("Prefabs/starParent");
		GameObject 	firstGradeStarPrefab = 	Resources.Load<GameObject> ("Prefabs/firstGradeStarParent");
		starParams[] starSo = 				Resources.LoadAll<starParams>("starDataScriptableObject");

		GameObject prefabBuffer;
		GameObject childObj;

		foreach(starParams sp in starSo){
			prefabBuffer = Instantiate (starPrefab, Vector3.zero, Quaternion.Euler(new Vector3(sp.dec, sp.ra, 0f))) as GameObject;

			prefabBuffer.transform.SetParent (skyObj.transform);

			childObj = prefabBuffer.transform.FindChild("starParticle").gameObject;

			childObj.transform.localScale *= (1f - sp.vMag * 0.1f);

			ParticleSystem ps = childObj.GetComponent<ParticleSystem> ();

			sp.starCol *= (1f - sp.vMag * 0.125f);

			ps.startColor = sp.starCol;

		}
	}
	[MenuItem ("Assets/CreateStarObjectsForSingle")]
	static void CreateStarObjectsForSingle(){
		GameObject 	skyObj = 				GameObject.Find ("blackSky");
		if (skyObj == null) {
			skyObj = Instantiate (Resources.Load<GameObject>("Prefabs/blackSky"));
		}
		GameObject 	starPrefab = 			Resources.Load<GameObject> ("Prefabs/starParent");
		GameObject 	firstGradeStarPrefab = 	Resources.Load<GameObject> ("Prefabs/firstGradeStarParent");
		starParamList spl	= 				Resources.Load<starParamList> ("starDataScriptableObject/starParams");

		GameObject prefabBuffer;
		GameObject childObj;

		foreach(starParamsGlue sp in spl.starParam){
			prefabBuffer = Instantiate (starPrefab, Vector3.zero, Quaternion.Euler(new Vector3(sp.dec, sp.ra, 0f))) as GameObject;
			prefabBuffer.transform.SetParent (skyObj.transform);

			childObj = prefabBuffer.transform.FindChild("starParticle").gameObject;

			childObj.transform.localScale *= (1f - sp.vMag * 0.1f);

			ParticleSystem ps = childObj.GetComponent<ParticleSystem> ();

			sp.starCol *= (1f - sp.vMag * 0.125f);

			ps.startColor = sp.starCol;

		}
	}
	static void registStar(StringReader reader){
		starParamsGlue sp = new starParamsGlue();
		string line = reader.ReadLine();
		List<string> starParamsString = new List<string> ();
		starParamsString = line.Split ('|').ToList();
		if (starParamsString.Count < 5)
			starParamsString.Add ("");
		sp.name = starParamsString [1].Replace (" ", "");

		List<string> ra = new List<string>();
		ra = starParamsString [2].Split (' ').ToList();
		sp.ra = raToDigree (ra.ToList());
		sp.dec = decToDigree(starParamsString [3].Replace (".", "").Split(' ').ToList());
		string spectType = starParamsString [4].Replace (" ", "");
		sp.starCol = spectalToCol (spectType);
		starParamsString [5] = starParamsString[5].Replace (" ", "");
		if (starParamsString [5] == "") {
			starParamsString [5] = "10.0";
		}
		sp.vMag = float.Parse(starParamsString[5]);
		convertScriptableObject (sp);
	}


	static void registStarForSingle(StringReader reader){
		starParamsGlue sp = new starParamsGlue();
		string line = reader.ReadLine();
		List<string> starParamsString = new List<string> ();
		starParamsString = line.Split ('|').ToList();
		if (starParamsString.Count < 5)
			starParamsString.Add ("");
		sp.name = starParamsString [1].Replace (" ", "");

		List<string> ra = new List<string>();
		ra = starParamsString [2].Split (' ').ToList();
		List<string> dec = new List<string>();
		dec = starParamsString [3].Split (' ').ToList();
		sp.ra = raToDigree (ra.ToList());
		sp.dec = decToDigree (dec.ToList ());
		string spectType = starParamsString [4].Replace (" ", "");
		sp.starCol = spectalToCol (spectType);
		starParamsString [5] = starParamsString[5].Replace (" ", "");
		if (starParamsString [5] == "") {
			starParamsString [5] = "10.0";
		}
		sp.vMag = float.Parse(starParamsString[5]);
		splItem.starParam.Add(sp);
	}
		
	static float raToDigree(List<string> rightAscensionList){
		return 
			float.Parse(rightAscensionList [0]) * 15f +	
			float.Parse(rightAscensionList [1]) * 15f / 60f +
			float.Parse(rightAscensionList [2]) * 15f / 60f / 60f;
	}
	static float decToDigree(List<string> declinationList){
		return float.Parse(declinationList [0] + "." + declinationList [1] + declinationList [2].Replace(".", ""));
	}
	static void convertScriptableObject(starParamsGlue sp){
		starParams item = ScriptableObject.CreateInstance<starParams> ();

		item.name = 		sp.name;
		item.ra = 			sp.ra;
		item.dec = 			sp.dec;
		item.starCol = 		sp.starCol;
		item.vMag = 		sp.vMag;

		string path = AssetDatabase.GenerateUniqueAssetPath("Assets/Resources/starDataScriptableObject/" + GenerateStarCnt + ".asset");

		AssetDatabase.CreateAsset(item, path);
		AssetDatabase.SaveAssets();
		AssetDatabase.Refresh();
		Debug.Log (item.name + " Output Compleate!!");

		GenerateStarCnt++;
	}
	static List<Color> spectalColorList = new List<Color>(){
		new Color(0.6f , 	0.68f, 1f),
		new Color(0.66f , 	0.74f, 1f),
		new Color(0.78f , 	0.83f, 1f),
		new Color(0.96f , 	0.96f, 1f),
		new Color(1f , 		0.95f, 0.91f),
		new Color(1f , 		0.82f, 0.62f),
		new Color(1f , 		0.79f, 0.43f),
		new Color(1f, 		0.6f, 0.3f)
	};
	static Dictionary<char, int> spectalCharDic = new Dictionary<char, int>(){{'O', 0}, {'B', 1}, {'A', 2}, {'F', 3}, {'G', 4}, {'K', 5}, {'M', 6}, {'L', 7}};

	static private Color spectalToCol(string spectType){
		//O 155 176 255
		//B 170 191 255
		//A 202 215 255
		//F 248 247 255
		//G 255 244 234
		//K 255 210 161
		//M 255 204 111

		if(spectType == "" ||
			(
				spectType[0] != 'O' && 
				spectType[0] != 'B' &&
				spectType[0] != 'A' &&
				spectType[0] != 'F' &&
				spectType[0] != 'G' &&
				spectType[0] != 'K' &&
				spectType[0] != 'M'
			)
		)	spectType = "F0";

		Color starCol;

		starCol = spectalColorList[spectalCharDic [spectType [0]]];
		Color highLevelStarCol = spectalColorList [spectalCharDic [spectType [0]] + 1];
		if (spectType.Length == 1) {
			spectType += '0';
		}
		if (!Char.IsDigit(spectType [1])) {
			return starCol;
		}
		return starCol + ((starCol - highLevelStarCol) / 10 * Int32.Parse (spectType [1].ToString()));
	}
}
[System.SerializableAttribute]
public class starParamsGlue{
	public string  name;
	public float ra;
	public float dec;
	public Color starCol;
	public float vMag;
}