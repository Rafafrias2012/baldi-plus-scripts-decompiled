using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001F3 RID: 499
[CreateAssetMenu(fileName = "Level Data", menuName = "Custom Assets/Level Data Object", order = 3)]
public class LevelObject : ScriptableObject
{
	// Token: 0x04000CFD RID: 3325
	public LevelObject[] previousLevels = new LevelObject[0];

	// Token: 0x04000CFE RID: 3326
	[Header("Initial Map Settings")]
	public IntVector2 minSize = new IntVector2(25, 25);

	// Token: 0x04000CFF RID: 3327
	public IntVector2 maxSize = new IntVector2(100, 100);

	// Token: 0x04000D00 RID: 3328
	public int minPlots = 5;

	// Token: 0x04000D01 RID: 3329
	public int maxPlots = 10;

	// Token: 0x04000D02 RID: 3330
	public int minPlotSize = 5;

	// Token: 0x04000D03 RID: 3331
	public int outerEdgeBuffer = 5;

	// Token: 0x04000D04 RID: 3332
	[Header("Hall settings")]
	public int minHallsToRemove = 2;

	// Token: 0x04000D05 RID: 3333
	public int maxHallsToRemove = 5;

	// Token: 0x04000D06 RID: 3334
	public int minSideHallsToRemove;

	// Token: 0x04000D07 RID: 3335
	public int maxSideHallsToRemove = 1;

	// Token: 0x04000D08 RID: 3336
	public int minReplacementHalls = 2;

	// Token: 0x04000D09 RID: 3337
	public int maxReplacementHalls = 5;

	// Token: 0x04000D0A RID: 3338
	public int bridgeTurnChance = 2;

	// Token: 0x04000D0B RID: 3339
	public int additionTurnChance = 5;

	// Token: 0x04000D0C RID: 3340
	public int maxHallAttempts = 3;

	// Token: 0x04000D0D RID: 3341
	public int deadEndBuffer = 6;

	// Token: 0x04000D0E RID: 3342
	public bool includeBuffers;

	// Token: 0x04000D0F RID: 3343
	public WeightedTexture2D[] hallWallTexs;

	// Token: 0x04000D10 RID: 3344
	public WeightedTexture2D[] hallFloorTexs;

	// Token: 0x04000D11 RID: 3345
	public WeightedTexture2D[] hallCeilingTexs;

	// Token: 0x04000D12 RID: 3346
	public WeightedTransform[] hallLights;

	// Token: 0x04000D13 RID: 3347
	public int maxLightDistance = 8;

	// Token: 0x04000D14 RID: 3348
	public int minPrePlotSpecialHalls;

	// Token: 0x04000D15 RID: 3349
	public int minPostPlotSpecialHalls;

	// Token: 0x04000D16 RID: 3350
	public int maxPrePlotSpecialHalls = 4;

	// Token: 0x04000D17 RID: 3351
	public int maxPostPlotSpecialHalls = 4;

	// Token: 0x04000D18 RID: 3352
	public float prePlotSpecialHallChance = 0.25f;

	// Token: 0x04000D19 RID: 3353
	public float postPlotSpecialHallChance = 0.25f;

	// Token: 0x04000D1A RID: 3354
	public WeightedRoomAsset[] potentialPrePlotSpecialHalls;

	// Token: 0x04000D1B RID: 3355
	public WeightedRoomAsset[] potentialPostPlotSpecialHalls;

	// Token: 0x04000D1C RID: 3356
	public RandomHallBuilder[] standardHallBuilders;

	// Token: 0x04000D1D RID: 3357
	public int minSpecialBuilders = 2;

	// Token: 0x04000D1E RID: 3358
	public int maxSpecialBuilders = 5;

	// Token: 0x04000D1F RID: 3359
	public ObjectBuilder[] forcedSpecialHallBuilders = new ObjectBuilder[0];

	// Token: 0x04000D20 RID: 3360
	public WeightedObjectBuilder[] specialHallBuilders = new WeightedObjectBuilder[0];

	// Token: 0x04000D21 RID: 3361
	[Header("Room Settings")]
	public RoomGroup[] roomGroup = new RoomGroup[0];

	// Token: 0x04000D22 RID: 3362
	public int minClassRooms = 5;

	// Token: 0x04000D23 RID: 3363
	public int maxClassRooms = 10;

	// Token: 0x04000D24 RID: 3364
	public float classStickToHallChance = 1f;

	// Token: 0x04000D25 RID: 3365
	public WeightedRoomAsset[] potentialClassRooms;

	// Token: 0x04000D26 RID: 3366
	public int minFacultyRooms = 5;

	// Token: 0x04000D27 RID: 3367
	public int maxFacultyRooms = 10;

	// Token: 0x04000D28 RID: 3368
	public float facultyStickToHallChance = 0.75f;

	// Token: 0x04000D29 RID: 3369
	public WeightedRoomAsset[] potentialFacultyRooms;

	// Token: 0x04000D2A RID: 3370
	public int minExtraRooms = 5;

	// Token: 0x04000D2B RID: 3371
	public int maxExtraRooms = 10;

	// Token: 0x04000D2C RID: 3372
	public float extraStickToHallChance = 0.25f;

	// Token: 0x04000D2D RID: 3373
	public WeightedRoomAsset[] potentialExtraRooms;

	// Token: 0x04000D2E RID: 3374
	public int minOffices = 1;

	// Token: 0x04000D2F RID: 3375
	public int maxOffices = 1;

	// Token: 0x04000D30 RID: 3376
	public float officeStickToHallChance = 1f;

	// Token: 0x04000D31 RID: 3377
	public WeightedRoomAsset[] potentialOffices;

	// Token: 0x04000D32 RID: 3378
	public IntVector2 minRoomSize;

	// Token: 0x04000D33 RID: 3379
	public float centerWeightMultiplier = 25f;

	// Token: 0x04000D34 RID: 3380
	public float perimeterBase = 4f;

	// Token: 0x04000D35 RID: 3381
	public float dijkstraWeightValueMultiplier = 1f;

	// Token: 0x04000D36 RID: 3382
	public float dijkstraWeightPower = 1.4f;

	// Token: 0x04000D37 RID: 3383
	public float extraDoorChance = 0.15f;

	// Token: 0x04000D38 RID: 3384
	public float additionalHallDoorRequirementMultiplier = 4f;

	// Token: 0x04000D39 RID: 3385
	public float hallPriorityDampening = 3f;

	// Token: 0x04000D3A RID: 3386
	public WeightedTexture2D[] classWallTexs;

	// Token: 0x04000D3B RID: 3387
	public WeightedTexture2D[] classFloorTexs;

	// Token: 0x04000D3C RID: 3388
	public WeightedTexture2D[] classCeilingTexs;

	// Token: 0x04000D3D RID: 3389
	public WeightedTexture2D[] facultyWallTexs;

	// Token: 0x04000D3E RID: 3390
	public WeightedTexture2D[] facultyFloorTexs;

	// Token: 0x04000D3F RID: 3391
	public WeightedTexture2D[] facultyCeilingTexs;

	// Token: 0x04000D40 RID: 3392
	public WeightedTransform[] classLights;

	// Token: 0x04000D41 RID: 3393
	public WeightedTransform[] facultyLights;

	// Token: 0x04000D42 RID: 3394
	public WeightedTransform[] officeLights;

	// Token: 0x04000D43 RID: 3395
	public StandardDoorMats standardDoorMat;

	// Token: 0x04000D44 RID: 3396
	public int minSpecialRooms = 1;

	// Token: 0x04000D45 RID: 3397
	public int maxSpecialRooms = 2;

	// Token: 0x04000D46 RID: 3398
	public bool specialRoomsStickToEdge = true;

	// Token: 0x04000D47 RID: 3399
	public WeightedRoomAsset[] potentialSpecialRooms;

	// Token: 0x04000D48 RID: 3400
	public float windowChance = 0.5f;

	// Token: 0x04000D49 RID: 3401
	[Header("Global Settings")]
	public LightMode lightMode;

	// Token: 0x04000D4A RID: 3402
	[Range(0f, 100f)]
	public int standardLightStrength = 10;

	// Token: 0x04000D4B RID: 3403
	public Color standardLightColor = Color.white;

	// Token: 0x04000D4C RID: 3404
	public Color standardDarkLevel = new Color(32f, 32f, 16f);

	// Token: 0x04000D4D RID: 3405
	public WeightedNPC[] potentialBaldis;

	// Token: 0x04000D4E RID: 3406
	public int additionalNPCs = 1;

	// Token: 0x04000D4F RID: 3407
	public List<WeightedNPC> potentialNPCs;

	// Token: 0x04000D50 RID: 3408
	public NPC[] forcedNpcs = new NPC[0];

	// Token: 0x04000D51 RID: 3409
	public float posterChance = 1f;

	// Token: 0x04000D52 RID: 3410
	public WeightedPosterObject[] posters;

	// Token: 0x04000D53 RID: 3411
	[Header("Item Settings")]
	public WeightedItemObject[] items;

	// Token: 0x04000D54 RID: 3412
	public List<ItemObject> forcedItems;

	// Token: 0x04000D55 RID: 3413
	public WeightedItemObject[] potentialItems;

	// Token: 0x04000D56 RID: 3414
	public int maxItemValue = 750;

	// Token: 0x04000D57 RID: 3415
	public int highEndCutoff = 250;

	// Token: 0x04000D58 RID: 3416
	public int lowEndCutoff = 150;

	// Token: 0x04000D59 RID: 3417
	public float duplicateItemWeightReduction = 0.15f;

	// Token: 0x04000D5A RID: 3418
	public int maxAllowedUnusedValue = 100;

	// Token: 0x04000D5B RID: 3419
	public int singleEntranceItemVal = 10;

	// Token: 0x04000D5C RID: 3420
	public int noHallItemVal = 15;

	// Token: 0x04000D5D RID: 3421
	[Header("")]
	public int minEvents = 1;

	// Token: 0x04000D5E RID: 3422
	[Header("")]
	public int maxEvents = 3;

	// Token: 0x04000D5F RID: 3423
	public float initialEventGap = 60f;

	// Token: 0x04000D60 RID: 3424
	public float minEventGap = 60f;

	// Token: 0x04000D61 RID: 3425
	public float maxEventGap = 210f;

	// Token: 0x04000D62 RID: 3426
	public List<WeightedRandomEvent> randomEvents = new List<WeightedRandomEvent>();

	// Token: 0x04000D63 RID: 3427
	public int exitCount = 1;

	// Token: 0x04000D64 RID: 3428
	public Elevator elevatorPre;

	// Token: 0x04000D65 RID: 3429
	public RoomAsset elevatorRoom;

	// Token: 0x04000D66 RID: 3430
	public int hallBuffer = 2;

	// Token: 0x04000D67 RID: 3431
	public int edgeBuffer = 3;

	// Token: 0x04000D68 RID: 3432
	[Header("Store and Bonus Settings")]
	public int mapPrice = 250;

	// Token: 0x04000D69 RID: 3433
	public int totalShopItems = 4;

	// Token: 0x04000D6A RID: 3434
	public WeightedItemObject[] shopItems;

	// Token: 0x04000D6B RID: 3435
	public bool finalLevel;

	// Token: 0x04000D6C RID: 3436
	public float timeBonusLimit;

	// Token: 0x04000D6D RID: 3437
	public int timeBonusVal = 50;
}
