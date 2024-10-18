using Newtonsoft.Json.Linq;

public interface ISaveable
{
    JToken CaptureAsJToken();
    void RestoreFromJToken(JToken state);
}
