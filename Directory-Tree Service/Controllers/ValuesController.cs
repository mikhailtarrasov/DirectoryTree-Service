using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;
using DataAccess;

namespace Directory_Tree_Service.Controllers
{
    public class ValuesController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage Create(string id, string parentId)
        {
            var treeClient = new DirectoryTreeClient();

            var result = treeClient.CreateNode(id, parentId);
            var status = (HttpStatusCode)result.Status;

            return Request.CreateResponse(status, result.Note);
        }

        [HttpPut]
        public HttpResponseMessage Update(string id, string parentId)
        {
            var treeClient = new DirectoryTreeClient();

            var result = treeClient.UpdateNode(id, parentId);
            var status = (HttpStatusCode)result.Status;

            return Request.CreateResponse(status, result.Note);
        }

        [HttpDelete]
        public HttpResponseMessage Delete(string id)
        {
            var treeClient = new DirectoryTreeClient();

            var result = treeClient.DeleteSubtree(id);
            var status = (HttpStatusCode)result.Status;

            return Request.CreateResponse(status, result.Note);
        }

        [HttpGet]
        public HttpResponseMessage Get()
        {
            var treeClient = new DirectoryTreeClient();
            
            var result = treeClient.GetSubTree(null);
            var status = (HttpStatusCode)result.Status;

            return Request.CreateResponse(status, status == HttpStatusCode.OK ? ConvertValueToString(result.SubTree) : result.Note);
        }

        [HttpGet]
        public HttpResponseMessage Get(string id)
        {
            var treeClient = new DirectoryTreeClient();

            var result = treeClient.GetSubTree(id);
            var status = (HttpStatusCode) result.Status;

            return Request.CreateResponse(status, status == HttpStatusCode.OK ? ConvertValueToString(result.SubTree) : result.Note);
        }

        public string ConvertValueToString<T>(T result)
        {
            return new JavaScriptSerializer().Serialize(result);
        }
    }
}
