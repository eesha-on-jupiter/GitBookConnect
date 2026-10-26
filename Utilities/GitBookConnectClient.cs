using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GitBookConnect
{
    // HTTP client for the GitBook API — the only file that touches HttpClient
    // Every method is built from spec/call-library-validated.json
    public sealed class GitBookConnectClient
    {
        #region Constructor and setup

        private const string BaseUrl = "https://api.gitbook.com/v1";

        private readonly HttpClient _http;
        private readonly string _token;

        public GitBookConnectClient(string token)
        {
            _token = (token ?? "").Trim();
            _http = new HttpClient { BaseAddress = new Uri(BaseUrl) };
        }

        private HttpRequestMessage NewRequest(HttpMethod method, string relativeUrl)
        {
            var req = new HttpRequestMessage(method, relativeUrl);
            req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            return req;
        }

        #endregion

        #region Auth methods

        // get_current_user — GET /user. Requires a GitBook Personal Access Token, not an integration token.
        public async Task<Tuple<bool, string, string>> GetCurrentUserAsync()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(_token)) return Fail("Token is empty.");

                var req = NewRequest(HttpMethod.Get, "/user");
                var res = await _http.SendAsync(req).ConfigureAwait(false);
                string body = await res.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (!res.IsSuccessStatusCode)
                    return Fail("HTTP " + (int)res.StatusCode + " " + res.ReasonPhrase + "\n" + body);

                return Ok(body);
            }
            catch (Exception ex) { return Fail(ex.ToString()); }
        }

        #endregion

        #region Workspace methods

        // list_organizations — GET /orgs
        public async Task<Tuple<bool, string, string>> ListOrganizationsAsync()
        {
            try
            {
                var req = NewRequest(HttpMethod.Get, "/orgs");
                var res = await _http.SendAsync(req).ConfigureAwait(false);
                string body = await res.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (!res.IsSuccessStatusCode)
                    return Fail("HTTP " + (int)res.StatusCode + " " + res.ReasonPhrase + "\n" + body);

                return Ok(body);
            }
            catch (Exception ex) { return Fail(ex.ToString()); }
        }

        // get_organization — GET /orgs/{organizationId}
        public async Task<Tuple<bool, string, string>> GetOrganizationAsync(string organizationId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(organizationId)) return Fail("Organization ID is empty.");

                var req = NewRequest(HttpMethod.Get, "/orgs/" + Uri.EscapeDataString(organizationId));
                var res = await _http.SendAsync(req).ConfigureAwait(false);
                string body = await res.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (!res.IsSuccessStatusCode)
                    return Fail("HTTP " + (int)res.StatusCode + " " + res.ReasonPhrase + "\n" + body);

                return Ok(body);
            }
            catch (Exception ex) { return Fail(ex.ToString()); }
        }

        // list_spaces — GET /orgs/{organizationId}/spaces
        public async Task<Tuple<bool, string, string>> ListSpacesAsync(string organizationId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(organizationId)) return Fail("Organization ID is empty.");

                var req = NewRequest(HttpMethod.Get, "/orgs/" + Uri.EscapeDataString(organizationId) + "/spaces");
                var res = await _http.SendAsync(req).ConfigureAwait(false);
                string body = await res.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (!res.IsSuccessStatusCode)
                    return Fail("HTTP " + (int)res.StatusCode + " " + res.ReasonPhrase + "\n" + body);

                return Ok(body);
            }
            catch (Exception ex) { return Fail(ex.ToString()); }
        }

        // get_space — GET /spaces/{spaceId}
        public async Task<Tuple<bool, string, string>> GetSpaceAsync(string spaceId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(spaceId)) return Fail("Space ID is empty.");

                var req = NewRequest(HttpMethod.Get, "/spaces/" + Uri.EscapeDataString(spaceId));
                var res = await _http.SendAsync(req).ConfigureAwait(false);
                string body = await res.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (!res.IsSuccessStatusCode)
                    return Fail("HTTP " + (int)res.StatusCode + " " + res.ReasonPhrase + "\n" + body);

                return Ok(body);
            }
            catch (Exception ex) { return Fail(ex.ToString()); }
        }

        // list_collections — GET /orgs/{organizationId}/collections
        public async Task<Tuple<bool, string, string>> ListCollectionsAsync(string organizationId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(organizationId)) return Fail("Organization ID is empty.");

                var req = NewRequest(HttpMethod.Get, "/orgs/" + Uri.EscapeDataString(organizationId) + "/collections");
                var res = await _http.SendAsync(req).ConfigureAwait(false);
                string body = await res.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (!res.IsSuccessStatusCode)
                    return Fail("HTTP " + (int)res.StatusCode + " " + res.ReasonPhrase + "\n" + body);

                return Ok(body);
            }
            catch (Exception ex) { return Fail(ex.ToString()); }
        }

        // get_collection — GET /collections/{collectionId}
        public async Task<Tuple<bool, string, string>> GetCollectionAsync(string collectionId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(collectionId)) return Fail("Collection ID is empty.");

                var req = NewRequest(HttpMethod.Get, "/collections/" + Uri.EscapeDataString(collectionId));
                var res = await _http.SendAsync(req).ConfigureAwait(false);
                string body = await res.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (!res.IsSuccessStatusCode)
                    return Fail("HTTP " + (int)res.StatusCode + " " + res.ReasonPhrase + "\n" + body);

                return Ok(body);
            }
            catch (Exception ex) { return Fail(ex.ToString()); }
        }

        #endregion

        #region Content methods

        // get_published_content — GET /spaces/{spaceId}/content
        public async Task<Tuple<bool, string, string>> GetPublishedContentAsync(string spaceId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(spaceId)) return Fail("Space ID is empty.");

                var req = NewRequest(HttpMethod.Get, "/spaces/" + Uri.EscapeDataString(spaceId) + "/content");
                var res = await _http.SendAsync(req).ConfigureAwait(false);
                string body = await res.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (!res.IsSuccessStatusCode)
                    return Fail("HTTP " + (int)res.StatusCode + " " + res.ReasonPhrase + "\n" + body);

                return Ok(body);
            }
            catch (Exception ex) { return Fail(ex.ToString()); }
        }

        // list_pages — GET /spaces/{spaceId}/content/pages
        public async Task<Tuple<bool, string, string>> ListPagesAsync(string spaceId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(spaceId)) return Fail("Space ID is empty.");

                var req = NewRequest(HttpMethod.Get, "/spaces/" + Uri.EscapeDataString(spaceId) + "/content/pages");
                var res = await _http.SendAsync(req).ConfigureAwait(false);
                string body = await res.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (!res.IsSuccessStatusCode)
                    return Fail("HTTP " + (int)res.StatusCode + " " + res.ReasonPhrase + "\n" + body);

                return Ok(body);
            }
            catch (Exception ex) { return Fail(ex.ToString()); }
        }

        // get_page — GET /spaces/{spaceId}/content/page/{pageId}
        public async Task<Tuple<bool, string, string>> GetPageAsync(string spaceId, string pageId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(spaceId)) return Fail("Space ID is empty.");
                if (string.IsNullOrWhiteSpace(pageId)) return Fail("Page ID is empty.");

                var req = NewRequest(HttpMethod.Get, "/spaces/" + Uri.EscapeDataString(spaceId) + "/content/page/" + Uri.EscapeDataString(pageId));
                var res = await _http.SendAsync(req).ConfigureAwait(false);
                string body = await res.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (!res.IsSuccessStatusCode)
                    return Fail("HTTP " + (int)res.StatusCode + " " + res.ReasonPhrase + "\n" + body);

                return Ok(body);
            }
            catch (Exception ex) { return Fail(ex.ToString()); }
        }

        // get_page_by_path — GET /spaces/{spaceId}/content/path/{pagePath}
        // pagePath is a slash-free page slug (e.g. "api-test-page"), not an id.
        public async Task<Tuple<bool, string, string>> GetPageByPathAsync(string spaceId, string pagePath)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(spaceId)) return Fail("Space ID is empty.");
                if (string.IsNullOrWhiteSpace(pagePath)) return Fail("Page Path is empty.");

                var req = NewRequest(HttpMethod.Get, "/spaces/" + Uri.EscapeDataString(spaceId) + "/content/path/" + Uri.EscapeDataString(pagePath));
                var res = await _http.SendAsync(req).ConfigureAwait(false);
                string body = await res.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (!res.IsSuccessStatusCode)
                    return Fail("HTTP " + (int)res.StatusCode + " " + res.ReasonPhrase + "\n" + body);

                return Ok(body);
            }
            catch (Exception ex) { return Fail(ex.ToString()); }
        }

        #endregion

        #region Change Request methods

        // create_change_request — POST /spaces/{spaceId}/change-requests
        // Body schema is { subject, template } only — GitBook does not accept a description at creation
        // time (description is set afterward via update_change_request). 'template' is omitted (optional).
        public async Task<Tuple<bool, string, string>> CreateChangeRequestAsync(string spaceId, string subject)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(spaceId)) return Fail("Space ID is empty.");

                var payload = new JObject { ["subject"] = subject ?? "" };
                var req = NewRequest(HttpMethod.Post, "/spaces/" + Uri.EscapeDataString(spaceId) + "/change-requests");
                req.Content = new StringContent(payload.ToString(Formatting.None), Encoding.UTF8, "application/json");

                var res = await _http.SendAsync(req).ConfigureAwait(false);
                string body = await res.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (!res.IsSuccessStatusCode)
                    return Fail("HTTP " + (int)res.StatusCode + " " + res.ReasonPhrase + "\n" + body);

                return Ok(body);
            }
            catch (Exception ex) { return Fail(ex.ToString()); }
        }

        // list_change_requests — GET /spaces/{spaceId}/change-requests
        public async Task<Tuple<bool, string, string>> ListChangeRequestsAsync(string spaceId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(spaceId)) return Fail("Space ID is empty.");

                var req = NewRequest(HttpMethod.Get, "/spaces/" + Uri.EscapeDataString(spaceId) + "/change-requests");
                var res = await _http.SendAsync(req).ConfigureAwait(false);
                string body = await res.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (!res.IsSuccessStatusCode)
                    return Fail("HTTP " + (int)res.StatusCode + " " + res.ReasonPhrase + "\n" + body);

                return Ok(body);
            }
            catch (Exception ex) { return Fail(ex.ToString()); }
        }

        // get_change_request — GET /spaces/{spaceId}/change-requests/{changeRequestId}
        public async Task<Tuple<bool, string, string>> GetChangeRequestAsync(string spaceId, string changeRequestId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(spaceId)) return Fail("Space ID is empty.");
                if (string.IsNullOrWhiteSpace(changeRequestId)) return Fail("Change Request ID is empty.");

                var req = NewRequest(HttpMethod.Get, "/spaces/" + Uri.EscapeDataString(spaceId) + "/change-requests/" + Uri.EscapeDataString(changeRequestId));
                var res = await _http.SendAsync(req).ConfigureAwait(false);
                string body = await res.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (!res.IsSuccessStatusCode)
                    return Fail("HTTP " + (int)res.StatusCode + " " + res.ReasonPhrase + "\n" + body);

                return Ok(body);
            }
            catch (Exception ex) { return Fail(ex.ToString()); }
        }

        // update_change_request — PATCH /spaces/{spaceId}/change-requests/{changeRequestId}
        // HttpMethod.Patch does not exist on net48 (Rhino 7) — use new HttpMethod("PATCH").
        public async Task<Tuple<bool, string, string>> UpdateChangeRequestAsync(string spaceId, string changeRequestId, string subject, string description)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(spaceId)) return Fail("Space ID is empty.");
                if (string.IsNullOrWhiteSpace(changeRequestId)) return Fail("Change Request ID is empty.");

                var payload = new JObject();
                if (!string.IsNullOrEmpty(subject)) payload["subject"] = subject;
                if (!string.IsNullOrEmpty(description)) payload["description"] = new JObject { ["markdown"] = description };

                var req = NewRequest(new HttpMethod("PATCH"), "/spaces/" + Uri.EscapeDataString(spaceId) + "/change-requests/" + Uri.EscapeDataString(changeRequestId));
                req.Content = new StringContent(payload.ToString(Formatting.None), Encoding.UTF8, "application/json");

                var res = await _http.SendAsync(req).ConfigureAwait(false);
                string body = await res.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (!res.IsSuccessStatusCode)
                    return Fail("HTTP " + (int)res.StatusCode + " " + res.ReasonPhrase + "\n" + body);

                return Ok(body);
            }
            catch (Exception ex) { return Fail(ex.ToString()); }
        }

        // get_change_request_content — GET /spaces/{spaceId}/change-requests/{changeRequestId}/content
        public async Task<Tuple<bool, string, string>> GetChangeRequestContentAsync(string spaceId, string changeRequestId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(spaceId)) return Fail("Space ID is empty.");
                if (string.IsNullOrWhiteSpace(changeRequestId)) return Fail("Change Request ID is empty.");

                var req = NewRequest(HttpMethod.Get, "/spaces/" + Uri.EscapeDataString(spaceId) + "/change-requests/" + Uri.EscapeDataString(changeRequestId) + "/content");
                var res = await _http.SendAsync(req).ConfigureAwait(false);
                string body = await res.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (!res.IsSuccessStatusCode)
                    return Fail("HTTP " + (int)res.StatusCode + " " + res.ReasonPhrase + "\n" + body);

                return Ok(body);
            }
            catch (Exception ex) { return Fail(ex.ToString()); }
        }

        // apply_change_request_content — POST /spaces/{spaceId}/change-requests/{changeRequestId}/content
        // changesJson is a pre-assembled JSON array string (from Content Changes Aggregator), 1-50 items.
        public async Task<Tuple<bool, string, string>> ApplyContentChangesAsync(string spaceId, string changeRequestId, string changesJson)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(spaceId)) return Fail("Space ID is empty.");
                if (string.IsNullOrWhiteSpace(changeRequestId)) return Fail("Change Request ID is empty.");
                if (string.IsNullOrWhiteSpace(changesJson)) return Fail("Change JSON is empty.");

                JArray changes;
                try { changes = JArray.Parse(changesJson); }
                catch (Exception ex) { return Fail("Change JSON is not a valid JSON array: " + ex.Message); }

                var payload = new JObject { ["changes"] = changes };
                var req = NewRequest(HttpMethod.Post, "/spaces/" + Uri.EscapeDataString(spaceId) + "/change-requests/" + Uri.EscapeDataString(changeRequestId) + "/content");
                req.Content = new StringContent(payload.ToString(Formatting.None), Encoding.UTF8, "application/json");

                var res = await _http.SendAsync(req).ConfigureAwait(false);
                string body = await res.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (!res.IsSuccessStatusCode)
                    return Fail("HTTP " + (int)res.StatusCode + " " + res.ReasonPhrase + "\n" + body);

                return Ok(body);
            }
            catch (Exception ex) { return Fail(ex.ToString()); }
        }

        // sync_change_request — POST /spaces/{spaceId}/change-requests/{changeRequestId}/update, no body.
        // HTTP 409 "Branch is already up-to-date" is GitBook's correct response when there is nothing to
        // rebase — treated as a non-error outcome here, not surfaced through Fail().
        public async Task<Tuple<bool, string, string>> SyncChangeRequestAsync(string spaceId, string changeRequestId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(spaceId)) return Fail("Space ID is empty.");
                if (string.IsNullOrWhiteSpace(changeRequestId)) return Fail("Change Request ID is empty.");

                var req = NewRequest(HttpMethod.Post, "/spaces/" + Uri.EscapeDataString(spaceId) + "/change-requests/" + Uri.EscapeDataString(changeRequestId) + "/update");
                var res = await _http.SendAsync(req).ConfigureAwait(false);
                string body = await res.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (res.StatusCode == System.Net.HttpStatusCode.Conflict)
                    return Ok(body);

                if (!res.IsSuccessStatusCode)
                    return Fail("HTTP " + (int)res.StatusCode + " " + res.ReasonPhrase + "\n" + body);

                return Ok(body);
            }
            catch (Exception ex) { return Fail(ex.ToString()); }
        }

        // request_reviewers — POST /spaces/{spaceId}/change-requests/{changeRequestId}/requested-reviewers
        public async Task<Tuple<bool, string, string>> RequestReviewersAsync(string spaceId, string changeRequestId, System.Collections.Generic.IEnumerable<string> userIds)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(spaceId)) return Fail("Space ID is empty.");
                if (string.IsNullOrWhiteSpace(changeRequestId)) return Fail("Change Request ID is empty.");

                var users = new JArray();
                foreach (var id in userIds ?? Array.Empty<string>())
                    if (!string.IsNullOrWhiteSpace(id)) users.Add(id);

                if (users.Count == 0) return Fail("Reviewer User ID list is empty.");

                var payload = new JObject { ["users"] = users };
                var req = NewRequest(HttpMethod.Post, "/spaces/" + Uri.EscapeDataString(spaceId) + "/change-requests/" + Uri.EscapeDataString(changeRequestId) + "/requested-reviewers");
                req.Content = new StringContent(payload.ToString(Formatting.None), Encoding.UTF8, "application/json");

                var res = await _http.SendAsync(req).ConfigureAwait(false);
                string body = await res.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (!res.IsSuccessStatusCode)
                    return Fail("HTTP " + (int)res.StatusCode + " " + res.ReasonPhrase + "\n" + body);

                return Ok(body);
            }
            catch (Exception ex) { return Fail(ex.ToString()); }
        }

        // merge_change_request — POST /spaces/{spaceId}/change-requests/{changeRequestId}/merge, no body.
        // Irreversible: publishes the change request's draft content into the space's primary content.
        public async Task<Tuple<bool, string, string>> MergeChangeRequestAsync(string spaceId, string changeRequestId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(spaceId)) return Fail("Space ID is empty.");
                if (string.IsNullOrWhiteSpace(changeRequestId)) return Fail("Change Request ID is empty.");

                var req = NewRequest(HttpMethod.Post, "/spaces/" + Uri.EscapeDataString(spaceId) + "/change-requests/" + Uri.EscapeDataString(changeRequestId) + "/merge");
                var res = await _http.SendAsync(req).ConfigureAwait(false);
                string body = await res.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (!res.IsSuccessStatusCode)
                    return Fail("HTTP " + (int)res.StatusCode + " " + res.ReasonPhrase + "\n" + body);

                return Ok(body);
            }
            catch (Exception ex) { return Fail(ex.ToString()); }
        }

        #endregion

        #region Private helpers

        private static Tuple<bool, string, string> Ok(string body)
            => new Tuple<bool, string, string>(true, body, "");

        private static Tuple<bool, string, string> Fail(string error)
            => new Tuple<bool, string, string>(false, "", error);

        #endregion
    }
}
