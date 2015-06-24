package com.freerdp.freerdpcore.model;

import org.json.JSONException;
import org.json.JSONObject;

public class ConnectionModel {

	public String id;
	public String protocol;
	
	public String hostname;
	public String username;
	public String disableAuth;
	public String password;
	public String remoteApp;
	public String port;
	public String rdpToken;
	
	public static final String TAG = "ConnectionModel";

	public ConnectionModel() {

	}

	public ConnectionModel (JSONObject jsonData) {
		try {			
			JSONObject data = jsonData;

			if (data.has("parameters")) {
				JSONObject parameters = data.getJSONObject("parameters");
				if (parameters.has("hostname")) {
					hostname = parameters.getString("hostname");
				}
				if (parameters.has("username")) {
					username = parameters.getString("username");
				}
				if (parameters.has("disable-auth")) {
					disableAuth = parameters.getString("disable-auth");
				}
				if (parameters.has("password")) {
					password = parameters.getString("password");
				}
				if (parameters.has("remote-app")) {
					remoteApp = parameters.getString("remote-app");
				}
				if (parameters.has("port")) {
					port = parameters.getString("port");
				}
				if (parameters.has("rdp-token")) {
					rdpToken = parameters.getString("rdp-token");
				}
			}
			if (data.has("id")) {
				id = data.getString("id");
			}
			if (data.has("rdp")) {
				protocol = data.getString("rdp");
			}
			
		} catch (JSONException e) {
			e.printStackTrace();
		}		
	}
}
