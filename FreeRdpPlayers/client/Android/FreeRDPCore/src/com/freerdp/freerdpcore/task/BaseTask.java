package com.freerdp.freerdpcore.task;

import android.os.AsyncTask;

public class BaseTask extends AsyncTask<Integer, Object, Object> {

	protected TaskListener mListener = null;
	protected int mTaskId = 0;
	private boolean mIsRunning = false;
	private Object mData = null;

	public BaseTask() {
		super();
	}

	public BaseTask(int taskId) {
		super();
		this.mTaskId = taskId;
	}

	public BaseTask(int taskId, Object data) {
		super();
		this.mTaskId = taskId;
		this.mData = data;
	}

	public void setListener(TaskListener listener) {
		mListener = listener;
	}

	@Override
	protected Object doInBackground(Integer... params) {
		mIsRunning = true;
		Object result = doRunning();
		mIsRunning = false;
		return result;
	}

	@Override
	protected void onCancelled() {
		super.onCancelled();
		mIsRunning = false;
	}

	@Override
	protected void onPostExecute(Object result) {
		super.onPostExecute(result);

		processResultListener(result);

		mIsRunning = false;
	}

	@Override
	protected void onProgressUpdate(Object... values) {
		super.onProgressUpdate(values);
		if (mListener != null) {
			mListener.onTaskProgress(mTaskId, values[0]);
		}
	}

	public void progress(Object value) {
		publishProgress(value);
	}

	protected void processResultListener(Object result) {
		if (mListener != null) {
			mListener.onTaskResult(mTaskId, result);
			mListener = null;
		}
	}

	public void release() {
		if (mIsRunning) {
			this.cancel(false);
			mIsRunning = false;
		}
	}

	protected Object doRunning() {
		if (mListener != null) {
			return mListener.onTaskRunning(mTaskId, mData);
		}
		return null;
	}

}
