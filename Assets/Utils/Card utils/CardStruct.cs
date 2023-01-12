using Firebase.Firestore;
[FirestoreData]
public struct CardStruct
{
    [FirestoreProperty]
    public string Name{ get; set; }
    [FirestoreProperty]
    public string Description{ get; set; }
    [FirestoreProperty]
    public int Health{ get; set; }
    [FirestoreProperty]
    public int Speed{ get; set; }
    [FirestoreProperty]
    public int Damage{ get; set; }
    [FirestoreProperty]
    public int Mana_Cost{ get; set; }
    [FirestoreProperty]
    public string Type{ get; set; }
    [FirestoreProperty]
    public string Ability_Type{ get; set; }
}