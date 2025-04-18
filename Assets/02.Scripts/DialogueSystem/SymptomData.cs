using System.Collections.Generic;
using System;

[System.Flags]
public enum Symptom
{
    None = 0,
    Dizzy = 1 << 0,
    Prickly = 1 << 1,
    Tickling = 1 << 2,
    Sneezing = 1 << 3,
     
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
    }
}