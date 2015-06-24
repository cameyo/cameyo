package com.freerdp.freerdpcore.model;

import org.json.JSONException;
import org.json.JSONObject;

public class AppModel {

	public String pkgId;
	public String appId;
	public String infoStr;
	public String iconUrl;
	

	public static final String TAG = "AppModel";

	public AppModel() {

	}

	public AppModel (JSONObject jsonData) {
		try {			
			JSONObject data = jsonData;

			if (data.has("PkgId")) {
				pkgId = data.getString("PkgId");
			}
			if (data.has("AppID")) {
				appId = data.getString("AppID");
			}
			if (data.has("InfoStr")) {
				infoStr = data.getString("InfoStr");
			}
			if (data.has("IconUrl")) {
				iconUrl = data.getString("IconUrl");
			}
		} catch (JSONException e) {
			e.printStackTrace();
		}		
	}
}
