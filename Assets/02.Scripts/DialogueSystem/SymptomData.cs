using System.Collections.Generic;
using System;

[System.Flags]
public enum Symptom
{
    None = 0,
    Dizzy = 1 << 0,            // 빙글빙글
    Prickly = 1 << 1,          // 따끔따끔
    Tickling = 1 << 2,         // 간질간질
    Sneezing = 1 << 3,         // 재채기 와르르
    RunnyNose = 1 << 4,        // 콧물 주르륵
    WarmFeel = 1 << 5,         // 따끈따끈
    Drowsy = 1 << 6,           // 꾸벅꾸벅
    ColdHandsFeet = 1 << 7,    // 손발 꽁꽁
    TickleThroat = 1 << 8,
    PricklyThroat = 1 << 9

}

namespace SymptomSystem
{
    public static class SymptomFlag
    {
        public static bool Has(Symptom data, Symptom symptom)
            => (data & symptom) == symptom;

        public static Symptom Add(this Symptom currentSymptom, Symptom flagToAdd)
        {
            return currentSymptom | flagToAdd;
        }

        public static Symptom Remove(this Symptom currentSymptom, Symptom flagToRemove)
        {
            return currentSymptom & ~flagToRemove;
        }
        public static List<Symptom> Extract(Symptom data)
        {
            List<Symptom> result = new List<Symptom>();
            foreach (Symptom s in Enum.GetValues(typeof(Symptom)))
            {
                if (s == Symptom.None) continue;

                if (Has(data, s))
                    result.Add(s);
            }
            return result;
        }
        public static bool IsVisualSymptom(Symptom symptom)
        {
            return symptom == Symptom.RunnyNose ||
                   symptom == Symptom.WarmFeel ||
                   symptom == Symptom.Drowsy ||
                   symptom == Symptom.ColdHandsFeet;
        }


        public static List<Symptom> ExtractVisual(Symptom data)
        {
            List<Symptom> all = Extract(data);
            return all.FindAll(IsVisualSymptom);
        }
    }
}