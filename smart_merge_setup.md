# Setting up Unity Smart Merge with SourceTree  
  
We have been primarily using SourceTree and Meld to manage version control. In order to allow for
smart merging of Unity Scenes and Prefabs some changes have to be made.  
  
### Steps:  
1. Go to (Tools -> Options -> Diff) in SourceTree
2. Set Merge tool to "Custom"  
3. The Diff Command should be `<Unity Install Path>\Editor\Data\Tools\UnityYAMLMerge.exe`
4. The Arguments should be `merge -p $BASE $REMOTE $LOCAL $MERGED`  
5. Open `mergespecificfile.txt` at `<Unity Install Path>\Editor\Data\Tools`  
6. Change:  
``` 
unity use "%programs%\YouFallbackMergeToolForScenesHere.exe" "%l" "%r" "%b" "%d"
prefab use "%programs%\YouFallbackMergeToolForPrefabsHere.exe" "%l" "%r" "%b" "%d"
```  
To:  
``` 
unity use "<Meld install path>\Meld.exe" "%l" "%r" "%b" "%d"
prefab use "<Meld install path>\Meld.exe" "%l" "%r" "%b" "%d"
```  
  
### You're all set!
To perform a smart merge, right click on the conflicted file and click Resolve Conflicts -> Launch External Merge Tool
