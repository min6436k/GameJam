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

namespace SymptomUtils
{
    public static class SymptomHelper
    {
        public static bool Has(Symptom data, Symptom symptom)
            => (data & symptom) == symptom;

        public static Symptom Add(Symptom current, Symptom toAdd)
            => current | toAdd;

        public static Symptom Remove(Symptom current, Symptom toRemove)
            => current & ~toRemove;

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