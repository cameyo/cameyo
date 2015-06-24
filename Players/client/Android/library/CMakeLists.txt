# FreeRDP: A Remote Desktop Protocol Implementation
# Android Client
#
# Copyright 2012 Marc-Andre Moreau <marcandre.moreau@gmail.com>
# Copyright 2013 Bernhard Miklautz <bernhard.miklautz@thincast.com>
#
# Licensed under the Apache License, Version 2.0 (the "License");
# you may not use this file except in compliance with the License.
# You may obtain a copy of the License at
#
#     http://www.apache.org/licenses/LICENSE-2.0
#
# Unless required by applicable law or agreed to in writing, software
# distributed under the License is distributed on an "AS IS" BASIS,
# WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
# See the License for the specific language governing permissions and
# limitations under the License.

set(ANDROID_PACKAGE_NAME "library")

CONFIGURE_FILE(${CMAKE_CURRENT_SOURCE_DIR}/AndroidManifest.xml.cmake
	${CMAKE_CURRENT_BINARY_DIR}/AndroidManifest.xml @ONLY)
CONFIGURE_FILE(${CMAKE_CURRENT_SOURCE_DIR}/build.xml.cmake
	${CMAKE_CURRENT_BINARY_DIR}/build.xml @ONLY)
CONFIGURE_FILE(${CMAKE_CURRENT_SOURCE_DIR}/project.properties.cmake
	${CMAKE_CURRENT_BINARY_DIR}/project.properties @ONLY)
CONFIGURE_FILE(${CMAKE_CURRENT_SOURCE_DIR}/ant.properties.cmake
	${CMAKE_CURRENT_BINARY_DIR}/ant.properties @ONLY)

if (ANDROID_SDK)
	CONFIGURE_FILE(${CMAKE_CURRENT_SOURCE_DIR}/local.properties.cmake
		${CMAKE_CURRENT_BINARY_DIR}/local.properties @ONLY)
endif()

SET_DIRECTORY_PROPERTIES(PROPERTIES ADDITIONAL_MAKE_CLEAN_FILES "gen;bin;obj;libs")

