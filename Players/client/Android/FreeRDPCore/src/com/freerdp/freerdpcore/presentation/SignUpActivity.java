package com.freerdp.freerdpcore.presentation;

import android.app.Activity;
import android.app.AlertDialog;
import android.app.ProgressDialog;
import android.content.DialogInterface;
import android.content.Intent;
import android.content.pm.ActivityInfo;
import android.os.Bundle;
import android.view.View;
import android.view.View.OnClickListener;
import android.widget.Button;
import android.widget.CheckBox;
import android.widget.EditText;

import com.freerdp.freerdpcore.R;
import com.freerdp.freerdpcore.manager.AppPreferences;
import com.freerdp.freerdpcore.manager.GlobalAPI;
import com.freerdp.freerdpcore.manager.Server;
import com.freerdp.freerdpcore.model.PackageCategoryModel;
import com.freerdp.freerdpcore.task.BaseTask;
import com.freerdp.freerdpcore.task.TaskListener;

public class SignUpActivity extends Activity implements TaskListener {
	private static final String TAG = "SignUpActivity";

	private ProgressDialog progressDialog;
	
	private Button m_SignInBtn;
	private Button m_SignUpBtn;
	
	private EditText m_EmailEdit;
	private EditText m_PasswordEdit;
	private EditText m_RePasswordEdit;
	
	private CheckBox m_SubscribeCheckBox;
	
	private String temporaryEmailStr;
	private String temporaryPassword;
	
	private static final int TASK_FORGETPASSSWORD = 2001;
	private static final int TASK_LOGIN = 2002;
	private static final int TASK_SIGNUP = 2003;

	@Override
	public void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		setContentView(R.layout.signup_activity);
		
		// Use actual screen orientation in order to avoid application crash on activity recreation after setRequestedOrientation.
		//boolean tabletSize = getResources().getBoolean(R.bool.isTablet);
		boolean tabletSize = getResources().getConfiguration().orientation == android.content.res.Configuration.ORIENTATION_LANDSCAPE;
		if (tabletSize) {
			setRequestedOrientation (ActivityInfo.SCREEN_ORIENTATION_LANDSCAPE);
		} else {
			setRequestedOrientation (ActivityInfo.SCREEN_ORIENTATION_PORTRAIT);
		}
		
		m_EmailEdit = (EditText)findViewById(R.id.email_edt);
		m_PasswordEdit = (EditText)findViewById(R.id.password_edt);
		m_RePasswordEdit = (EditText)findViewById(R.id.repassword_edt);
		
		m_SubscribeCheckBox = (CheckBox)findViewById(R.id.subscribenews_chk);
						
		// test
//		m_EmailEdit.setText("endribisek@outlook.com");
//		m_PasswordEdit.setText("endribisek");
//		m_RePasswordEdit.setText("endribisek");
		
		m_SignInBtn = (Button)findViewById(R.id.signin_btn);
		m_SignInBtn.setOnClickListener(new OnClickListener() {			
			@Override
			public void onClick(View v) {
				handleSignIn();
			}
		});
		
		m_SignUpBtn = (Button)findViewById(R.id.register_btn);
		m_SignUpBtn.setOnClickListener(new OnClickListener() {			
			@Override
			public void onClick(View v) {
				handleSignup();
			}
		});
	}

	private void handleSignIn() {
		Intent intent = new Intent(this, LoginActivity.class); 
		intent.setFlags(Intent.FLAG_ACTIVITY_NEW_TASK | Intent.FLAG_ACTIVITY_CLEAR_TASK);
		startActivity(intent);
	}
	
	private void handleSignup() {
		temporaryEmailStr = m_EmailEdit.getText().toString();
		temporaryPassword = m_PasswordEdit.getText().toString();
		
		if (temporaryEmailStr.length() == 0) {
			GlobalAPI.showDialogAlert(this, getString(R.string.alert_blankemail_exists));
			return;
		}
		
		if (!GlobalAPI.validEmail(temporaryEmailStr)) {
			GlobalAPI.showDialogAlert(this, getString(R.string.alert_invalid_email));
			return;
		}
		
		if (temporaryPassword.length() == 0) {
			GlobalAPI.showDialogAlert(this, getString(R.string.alert_blank_password));
			return;
		}
		
		if (!temporaryPassword.equals(m_RePasswordEdit.getText().toString())) {
			GlobalAPI.showDialogAlert(this, getString(R.string.notmatch_password));
			return;
		}
		
		progressDialog = GlobalAPI.showProgressDialog(this);
		BaseTask signupTask = new BaseTask(TASK_SIGNUP);
		signupTask.setListener(this);
		signupTask.execute();
		
	}
	
	@Override
	public Object onTaskRunning(int taskId, Object data) {
		if (taskId == TASK_FORGETPASSSWORD) {			
		} else if (taskId == TASK_LOGIN) {
			return Server.sendLoginRequest(temporaryEmailStr, temporaryPassword);
		} else if (taskId == TASK_SIGNUP) {
			return Server.sendSignUpRequest(temporaryEmailStr, temporaryPassword, m_SubscribeCheckBox.isChecked() ? "1":"0");
		}
		return null;
	}

	@Override
	public void onTaskResult(int taskId, Object result) {
		progressDialog.dismiss();
		if (taskId == TASK_FORGETPASSSWORD) {
			String retPassword = (String)result;
			if (retPassword != null && !retPassword.equals("")) {
				GlobalAPI.showDialogAlert(this, getString(R.string.alert_forgetpassword_sent));
			} else {
				GlobalAPI.showDialogAlert(this, getString(R.string.alert_emailaddress_exists_sent));
			}
		} else if (taskId == TASK_LOGIN) {
			PackageCategoryModel[] categoryArray = (PackageCategoryModel[])result;
			if (categoryArray != null && categoryArray.length > 0) {
				AppPreferences.getInstance(this).setLoEmailId(temporaryEmailStr);
				AppPreferences.getInstance(this).setLoPassword(temporaryPassword);
				AppPreferences.getInstance(this).currentCategories = categoryArray;
				Intent intent = new Intent(this, AppMainActivity.class);
				intent.setFlags(Intent.FLAG_ACTIVITY_NEW_TASK | Intent.FLAG_ACTIVITY_CLEAR_TASK);
				startActivity(intent);
				
			} else {
				GlobalAPI.showDialogAlert(this, R.string.alertmsg_title, R.string.alert_wrongusernameorpassword);
			}
		} else if (taskId == TASK_SIGNUP) {
			boolean ret = (Boolean)result;
			if (ret) {
				AlertDialog.Builder builder = new AlertDialog.Builder(SignUpActivity.this);
				builder.setMessage(getString(R.string.alert_signup_success))
					   .setTitle(getString(R.string.alertmsg_signup))
				       .setCancelable(true)
				       .setPositiveButton(android.R.string.ok, new DialogInterface.OnClickListener() {
				           public void onClick(DialogInterface dialog, int id) {
				        	   handleSignIn();
				           }
				       });
				AlertDialog alert = builder.create();
				alert.show();
			} else {
				GlobalAPI.showDialogAlert(this, R.string.alertmsg_title, R.string.alert_signupinvalid);
			}
		}
	}

	@Override
	public void onTaskProgress(int taskId, Object progress) {
		// TODO Auto-generated method stub
		
	}

	@Override
	public void onBackPressed() {
		handleSignIn();
//		super.onBackPressed();
	}
	
	
}
