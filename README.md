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
   ![image](https://github.com/user-attachments/assets/f9067ad7-98f1-4f76-bb2e-d9410d569d7d)
   ![image](https://github.com/user-attachments/assets/299adf28-3364-40c1-924d-6add06936f38)
   ![image](https://github.com/user-attachments/assets/47cf44a1-7002-43f8-9c68-d5046cb77035)

3. **Report Issues**: Select the "Report Issues" option to proceed.
   ![image](https://github.com/user-attachments/assets/b0321b5e-acf0-4dfe-8de0-7a5caa155ccc)

   - **Location Input**: Enter the location of the issue.
   ![image](https://github.com/user-attachments/assets/b0936349-f5e5-436e-bd09-75045a851501)
   ![image](https://github.com/user-attachments/assets/a514fd8f-b476-4c0d-92f2-c241b103aea7)

   - **Category Selection**: Choose a category from the dropdown (e.g., sanitation, roads, utilities).
   ![image](https://github.com/user-attachments/assets/80b27cd5-fc67-4fd3-a011-43e96c191ad1)

   - **Description**: Provide a detailed description of the issue.
   ![image](https://github.com/user-attachments/assets/ff49b878-2aa7-4be8-8efa-9b74b9df4324)
   ![image](https://github.com/user-attachments/assets/4af68092-2757-4621-827b-8669ace130ed)

   - **Media Attachment**: Click on the "Attach File" button to upload images, videos or documents related to the issue.
   ![image](https://github.com/user-attachments/assets/6561d1a9-13e0-4b49-a798-dcd784281d8c)

   - **Submit**: Once all fields are completed, the submit button will be enabled, click the "Submit" button to finalize your report.
5. **Feedback**: A message box will appear to confirm the successful submission or inform the user if any errors occur.
6. **In-app chat messaging function**: On the left side of the window, the submitted reports will display and allow the following message:
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

