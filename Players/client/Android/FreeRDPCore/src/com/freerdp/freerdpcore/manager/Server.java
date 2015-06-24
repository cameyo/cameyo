package com.freerdp.freerdpcore.manager;

import java.net.MalformedURLException;
import java.net.URI;
import java.net.URISyntaxException;
import java.net.URL;
import java.util.ArrayList;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import android.util.Log;

import com.freerdp.freerdpcore.model.AppModel;
import com.freerdp.freerdpcore.model.ConnectionModel;
import com.freerdp.freerdpcore.model.PackageCategoryModel;


/**
 * This class communicate with the server.
 */
public class Server {

	public static String serverUrl(String relativePage, String userEmail, String password) {
		String httpOrhttps = Protocol.SERVER_HTTPS ? "https://" : "http://";
		String url = httpOrhttps + Protocol.SERVER_URL + ":" + Protocol.SERVER_PORT;
        if (relativePage != null && !relativePage.isEmpty())
            url += "/" + relativePage;
		if (userEmail != null && !userEmail.isEmpty() && password != null && !password.isEmpty()) {
			if (relativePage.contains("?"))
				url += "&user=" + userEmail + "&pass=" + password;
			else
				url += "?user=" + userEmail + "&pass=" + password;
		}
		return url;
	}

	public static String serverUrlAuth(String relativePage, String userEmail, String password) {
		String httpOrhttps = Protocol.SERVER_HTTPS ? "https://" : "http://";
		String url = httpOrhttps + Protocol.SERVER_URL + ":" + Protocol.SERVER_PORT;
        if (relativePage != null && !relativePage.isEmpty())
            url += "/" + relativePage;
		if (userEmail != null && !userEmail.isEmpty() && password != null && !password.isEmpty()) {
			String auth = userEmail + "|" + password;
			String authEncoded = GlobalAPI.bytesToHex(auth.getBytes());
			if (relativePage.contains("?"))
				url += "&auth=" + authEncoded;
			else
				url += "?auth=" + authEncoded;
		}
		return url;
	}

    public static String serverUrl(String relativePage) {
        return serverUrl(relativePage, null, null);
    }

    public static String serverUrl() {
        return serverUrl(null, null, null);
    }

    public static PackageCategoryModel[] sendLoginRequest(String userEmail, String password) {
		String httpOrhttps = Protocol.SERVER_HTTPS ? "https://" : "http://";
		String path = httpOrhttps + Protocol.SERVER_URL + ":" + Protocol.SERVER_PORT
				+ "/packager.aspx?op=AccountAuth" + "&user=" + userEmail
				+ "&pass="  + password;

		try {
			URL url = new URL(path);
			URI uri = new URI(url.getProtocol(), url.getUserInfo(), url.getHost(), url.getPort(), url.getPath(), url.getQuery(), url.getRef());
			url = uri.toURL();
			path = url.toString();
		} catch (MalformedURLException e1) {
			e1.printStackTrace();
		} catch (URISyntaxException e) {
			e.printStackTrace();
		}

		Log.i("login", path);

		String response = HttpApi.sendRequest(path, null, null);

		if (response == null) return null;

		PackageCategoryModel[] retModelArray = null;
		try {

			if (response != null && !response.equals("")) {
				JSONObject origin = new JSONObject(response);
				JSONArray dataArray = origin.getJSONArray("libs");
				Log.i("array", dataArray.toString());
				if (dataArray != null && dataArray.length() > 0) {
					retModelArray = new PackageCategoryModel[dataArray.length() + 1];
					for (int i = 0; i < dataArray.length(); i ++) {
						JSONObject element = dataArray.getJSONObject(i);
						Log.i("element", element.toString());
						retModelArray[i] = new PackageCategoryModel(element);
					}
					retModelArray[dataArray.length()] = new PackageCategoryModel("[Add]", "Add your own", "");
				}
			}
		} catch (JSONException e) {
			e.printStackTrace();
		}
		return retModelArray;
	}

	public static boolean sendSignUpRequest(String userEmail, String password, String subscription) {
		String httpOrhttps = Protocol.SERVER_HTTPS ? "https://" : "http://";
		String path = httpOrhttps + Protocol.SERVER_URL + ":" + Protocol.SERVER_PORT
				+ "/packager.aspx?op=AccountCreate" + "&user=" + userEmail
				+ "&pass="  + password + "&subscribe=" + subscription;

		try {
			URL url = new URL(path);
			URI uri = new URI(url.getProtocol(), url.getUserInfo(), url.getHost(), url.getPort(), url.getPath(), url.getQuery(), url.getRef());
			url = uri.toURL();
			path = url.toString();
		} catch (MalformedURLException e1) {
			e1.printStackTrace();
		} catch (URISyntaxException e) {
			e.printStackTrace();
		}

		Log.i("login", path);

		String response = HttpApi.sendRequest(path, null, null);

		if (response == null || !response.equals("+OK")) return false;

		return true;
	}

	public static AppModel[] getAppArray(String userEmail, String password, String lib) {
		String httpOrhttps = Protocol.SERVER_HTTPS ? "https://" : "http://";
		String path = httpOrhttps + Protocol.SERVER_URL + ":" + Protocol.SERVER_PORT
				+ "/packager.aspx?op=PkgList" + "&user=" + userEmail
				+ "&pass="  + password + "&lib=" + lib + "&detail=Player";

		try {
			URL url = new URL(path);
			URI uri = new URI(url.getProtocol(), url.getUserInfo(), url.getHost(), url.getPort(), url.getPath(), url.getQuery(), url.getRef());
			url = uri.toURL();
			path = url.toString();
		} catch (MalformedURLException e1) {
			e1.printStackTrace();
		} catch (URISyntaxException e) {
			e.printStackTrace();
		}

		Log.i("login", path);

		String response = HttpApi.sendRequest(path, null, null);

		if (response == null) return null;

		AppModel[] retModelArray = null;
		try {

			if (response != null && !response.equals("")) {
				JSONArray dataArray = new JSONArray(response);
				Log.i("array", dataArray.toString());
				if (dataArray != null && dataArray.length() > 0) {
					retModelArray = new AppModel[dataArray.length()];
					for (int i = 0; i < dataArray.length(); i ++) {
						JSONObject element = dataArray.getJSONObject(i);
						Log.i("element", element.toString());
						retModelArray[i] = new AppModel(element);
					}
				}
			}
		} catch (JSONException e) {
			e.printStackTrace();
		}
		return retModelArray;
	}

	public static ConnectionModel sendConnectionRequest(String userEmail, String password, String packageId) {
		String httpOrhttps = Protocol.SERVER_HTTPS ? "https://" : "http://";
		String path = httpOrhttps + Protocol.SERVER_URL + ":" + Protocol.SERVER_PORT
				+ "/packager.aspx?op=RdpPlay" + "&user=" + userEmail
				+ "&pass="  + password + "&pkgId=" + packageId + "&client=Play.Android";

		try {
			URL url = new URL(path);
			URI uri = new URI(url.getProtocol(), url.getUserInfo(), url.getHost(), url.getPort(), url.getPath(), url.getQuery(), url.getRef());
			url = uri.toURL();
			path = url.toString();
		} catch (MalformedURLException e1) {
			e1.printStackTrace();
		} catch (URISyntaxException e) {
			e.printStackTrace();
		}

		Log.i("login", path);

		String response = HttpApi.sendRequest(path, null, null, 90 * 1000);

		if (response == null) return null;

		ConnectionModel retModel = null;
		try {

			if (response != null && !response.equals("") && !response.startsWith("ERR:")) {
				JSONObject origin = new JSONObject(response);
				retModel = new ConnectionModel(origin);
			}
		} catch (JSONException e) {
			e.printStackTrace();
		}
		return retModel;
	}

	public static boolean sendRdpStatusRequest(String token) {
		String httpOrhttps = Protocol.SERVER_HTTPS ? "https://" : "http://";
		String path = httpOrhttps + Protocol.SERVER_URL + ":" + Protocol.SERVER_PORT
				+ "/packager.aspx?op=RdpStatus" + "&token=" + token;

		try {
			URL url = new URL(path);
			URI uri = new URI(url.getProtocol(), url.getUserInfo(), url.getHost(), url.getPort(), url.getPath(), url.getQuery(), url.getRef());
			url = uri.toURL();
			path = url.toString();
		} catch (MalformedURLException e1) {
			e1.printStackTrace();
		} catch (URISyntaxException e) {
			e.printStackTrace();
		}

		Log.i("login", path);

		String response = HttpApi.sendRequest(path, null, null);

		if (response == null) return false;

		int retModel = Integer.valueOf(response);

		return retModel >= 4;
	}
}
