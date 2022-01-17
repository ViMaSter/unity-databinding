#nullable enable
using UnityEngine;

namespace DataBinding
{
    public abstract class SupportsRelativeDocumentPath : MonoBehaviour
    {
        public Document? Document;
        public string DocumentPath = "";
        public void PropagateParentSettings(Document? document, string path)
        {
            Debug.Assert(DocumentPath.Contains("$0"), "_documentPath cannot be updated, as it contains no relative qualifier '$0'");
            Document = document;
            DocumentPath = DocumentPath.Replace("$0", path);
        }
    }
}
