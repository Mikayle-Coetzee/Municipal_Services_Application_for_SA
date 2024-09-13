# ST10023767_PROG7312 (Municipal Services Application for South Africa)

We were given a POE where we needed to make a WPF Application, using C# and .NET Framework. The application is for a municipal services in South Africa. The application will allow users to report issues, request services, and access local event information. Initially, for part 1, the "Report Issues" functionality will be implemented to allow users to submit detailed reports about municipal issues such as sanitation, roads, and utilities.

## Author 
#### Mikayle Devpnique Coetzee
##### ST10023767


## Features (Phase 1)
- **Report Issues**: Allows users to report municipal issues by providing details such as location, category, description, and media attachments.
- **Local Events and Announcements**: This feature will be implemented in later phases.
- **Service Request Status**: This feature will also be implemented in later phases.

## Instructions for Compiling and Running the Application

### Prerequisites
Ensure you have the following:
- **Visual Studio 2019 or later** with .NET Framework installed.
- **.NET Framework 4.8** installed on your machine.

### Steps to Compile
1. **Download the source code**: Clone or download the project files to your local machine.
2. **Open Visual Studio**: Launch Visual Studio and select `Open a project or solution`.
3. **Locate the project**: Browse to the folder where the project is saved, and select the `.sln` file to open the solution.
4. **Restore NuGet packages**: If there are any missing dependencies, right-click on the solution in the Solution Explorer and select `Restore NuGet Packages`.
5. **Build the solution**: Once the solution is loaded, press `Ctrl + Shift + B` to build the project. Ensure that there are no build errors.

### Steps to Run
1. **Run the project**: Press `F5` to run the application in Debug mode or click on `Start` in the toolbar.
2. **Main Menu**: Upon startup, the main window will appear, providing three options:
   - **Report Issues (active)**
   - **Local Events and Announcements (disabled)**
   - **Service Request Status (disabled)**
3. **Report Issues**: Select the "Report Issues" option to proceed.
   - **Location Input**: Enter the location of the issue.
   - **Category Selection**: Choose a category from the dropdown (e.g., sanitation, roads, utilities).
   - **Description**: Provide a detailed description of the issue.
   - **Media Attachment**: Click on the "Attach File" button to upload images, videos or documents related to the issue.
   - **Submit**: Once all fields are completed, the submit button will be enabled, click the "Submit" button to finalize your report.
4. **Feedback**: A message box will appear to confirm the successful submission or inform the user if any errors occur.
5. **In-app chat messaging function**: On the left side of the window, the submitted reports will display and allow the following message:
   - **1/help**: will display a support message
   - **2/view**: will display all the submitted reports 
   - **3/new**: will start a new message 
   - **4/search**: will display the report with the same location and category entered

### Key Functions
- **Form Navigation**: Use navigation buttons (e.g., "Back to Main Menu") to move between different sections of the application.
- **Data Handling**: All reported issues will be stored in a local list, using appropriate data structures for efficient management.
- **User engagement**: Chat function and progress bar with motivational lables. 

## Design 
- **User-Centric Design**: The application is designed to be intuitive and easy to use, with clear labels and instructions.
- **Consistency**: A consistent colour scheme and layout are maintained throughout the application.
- **Responsiveness**: The interface is designed to work well across different screen sizes.

