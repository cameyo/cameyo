package com.freerdp.freerdpcore.manager;

import org.apache.http.HttpEntity;
import org.apache.http.HttpResponse;
import org.apache.http.client.CookieStore;
import org.apache.http.client.methods.HttpPost;
import org.apache.http.impl.client.DefaultHttpClient;
import org.apache.http.params.HttpConnectionParams;
import org.apache.http.protocol.HTTP;
import org.apache.http.util.EntityUtils;

import android.util.Log;

/**
 * Http communication API
 * @author Endri Bisek
 */
public class HttpApi {
	private static final String LOG = "minicabsclient.HttpApi";
	private static CookieStore cookieStore = null;

	public static String sendRequest(String serverUrl, HttpEntity entity, int[] statusCode) {
		return sendRequest(serverUrl, entity, statusCode, 10 * 1000);
	}

	public static String sendRequest(String serverUrl, HttpEntity entity, int[] statusCode, int soTimeout) {

		HttpPost httpRequest = new HttpPost(serverUrl);
		int status = -1;

		try {
			/** Makes an HTTP request request */
			if (entity != null) {
				httpRequest.setEntity(entity);
			}

			/** Create an HTTP client */
			DefaultHttpClient httpClient = new DefaultHttpClient();

			org.apache.http.params.HttpParams nparams = httpClient.getParams();
			HttpConnectionParams.setConnectionTimeout(nparams, 30 * 1000);
			HttpConnectionParams.setSoTimeout(nparams, soTimeout);
			//httpClient.setParams(params);
			/** Set Cookie information */
			if (cookieStore != null) {
				httpClient.setCookieStore(cookieStore);
			}

			/** Gets the HTTP response response */
//			Log.i("xxxxx", httpRequest.getURI().toString());
			HttpResponse httpresponse = httpClient.execute(httpRequest);

			status = httpresponse.getStatusLine().getStatusCode();

			/** If the status code 200 response successfully */
			Log.v(LOG, new Integer(status).toString());
			if (status == 200) {
				/** Remove the response string */
				String strResponse = EntityUtils.toString(httpresponse.getEntity(), HTTP.UTF_8);
				Log.v(LOG, strResponse);
				if (statusCode != null)
					statusCode[0] = status;

				cookieStore = httpClient.getCookieStore();
				return strResponse.trim();
			}
		} catch (Exception e) {
			Log.v(LOG, e.toString());
			Log.v(LOG, "send request error");
			status = -1;
		}

		if (statusCode != null)
			statusCode[0] = status;
		return null;
	}

	public static void clearCookie() {
		cookieStore = null;
	}


}
