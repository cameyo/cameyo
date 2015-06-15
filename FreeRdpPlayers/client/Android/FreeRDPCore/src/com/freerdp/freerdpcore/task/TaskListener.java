package com.freerdp.freerdpcore.task;

public interface TaskListener {
	public Object onTaskRunning(int taskId, Object data);
	public void onTaskResult(int taskId, Object result);
	public void onTaskProgress(int taskId, Object progress);
}