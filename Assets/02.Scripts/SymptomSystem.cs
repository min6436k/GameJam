using System;

[Flags]
public enum SymptomTag
{
    None = 0,
    RunnyNose = 1 << 0,
    Rash = 1 << 1,
    Cold = 1 << 2,
    a = 1 << 3,
    b = 1 << 4,
    c = 1 << 5

}

namespace SymptomSystem
{
    public static class SymptomFlag
    {
        public static SymptomTag Add(this SymptomTag currentSymptom, SymptomTag flagToAdd)
        {
            return currentSymptom | flagToAdd;
        }

        public static SymptomTag Remove(this SymptomTag currentSymptom, SymptomTag flagToRemove)
        {
            return currentSymptom & ~flagToRemove;
        }
    }
}