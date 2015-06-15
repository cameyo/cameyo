<?xml version="1.0" encoding="UTF-8"?>
<manifest xmlns:android="http://schemas.android.com/apk/res/android"
    package="com.cameyo.player"
    android:installLocation="auto"
	android:versionCode="@ANDROID_APP_VERSION@"
	android:versionName="@GIT_REVISION@" >

	<uses-sdk
		android:targetSdkVersion="@ANDROID_APP_TARGET_SDK@"
		android:minSdkVersion="@ANDROID_APP_MIN_SDK@"/>

	<supports-screens
		android:anyDensity="true"
		android:smallScreens="true"
		android:normalScreens="true"
		android:largeScreens="true"
		android:xlargeScreens="true" />
			
	<application android:name="com.freerdp.afreerdp.application.GlobalApp"
		android:label="Cameyo Player"
		android:debuggable="@ANDROID_DEBUG_ENABLE@"
		android:icon="@drawable/icon_launcher_freerdp" >

			<activity
            android:name="com.freerdp.freerdpcore.presentation.LoginActivity"
            android:theme="@style/Theme.Main" >
            <intent-filter android:label="@string/app_title" >
                <action android:name="android.intent.action.MAIN" />
                <category android:name="android.intent.category.LAUNCHER" />                
            </intent-filter>
        </activity>
        
        <activity
            android:name="com.freerdp.freerdpcore.presentation.AppMainActivity"            
            >
            <intent-filter>
                <action android:name="android.intent.action.MAIN" />
            </intent-filter>
        </activity>
        
        <activity
            android:name="com.freerdp.freerdpcore.presentation.SignUpActivity"
            android:theme="@style/Theme.Main" >
            <intent-filter android:label="@string/app_title" >
                <action android:name="android.intent.action.MAIN" />
            </intent-filter>
        </activity>
        
        <!-- Main activity -->
        <activity
            android:name="com.freerdp.freerdpcore.presentation.HomeActivity"
            android:alwaysRetainTaskState="true"
            android:label="@string/app_title"
            android:theme="@style/Theme.Main" >
            <intent-filter android:label="@string/app_title" >
                <action android:name="android.intent.action.MAIN" />
                
            </intent-filter>
            <intent-filter>
                <action android:name="android.intent.action.VIEW" />

                <category android:name="android.intent.category.DEFAULT" />
                <category android:name="android.intent.category.BROWSABLE" />

                <data
                    android:host="*"
                    android:pathPattern=".*\\.rdp"
                    android:scheme="http" />
                <data
                    android:host="*"
                    android:pathPattern=".*\\.rdp"
                    android:scheme="file" />
                <data android:mimeType="*/*" />
            </intent-filter>
        </activity>

        <!-- Session request handler activity - used for search and internally to start sessions -->
        <!-- This should actually be defined in FreeRDPCore lib but Android manifest merging will -->
        <!-- append the libs manifest to the apps manifest and therefore aliasing is not possible -->
        <activity
            android:name="com.freerdp.freerdpcore.services.SessionRequestHandlerActivity"
            android:excludeFromRecents="true"
            android:noHistory="true"
            android:theme="@android:style/Theme.NoDisplay" >
            <intent-filter>
                <action android:name="android.intent.action.MAIN" />
            </intent-filter>
        </activity>

        <activity-alias
            android:name=".services.SessionRequestHandlerActivity"
            android:targetActivity="com.freerdp.freerdpcore.services.SessionRequestHandlerActivity" >
            <intent-filter>
                <action android:name="android.intent.action.SEARCH" />
            </intent-filter>

            <meta-data
                android:name="android.app.searchable"
                android:resource="@xml/searchable" />
        </activity-alias>

        <provider
            android:name="com.freerdp.freerdpcore.services.FreeRDPSuggestionProvider"
            android:authorities="com.cameyo.player.services.freerdpsuggestionprovider" >
        </provider>
    </application>

</manifest>

