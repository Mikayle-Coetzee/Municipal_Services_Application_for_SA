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
   - ![image](https://github.com/user-attachments/assets/b0321b5e-acf0-4dfe-8de0-7a5caa155ccc)

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
   ![image](https://github.com/user-attachments/assets/2023a965-bdaa-4435-a069-8f045f87910e)
   ![image](https://github.com/user-attachments/assets/5083773a-4802-476b-acbb-0ae270e990e6)

   Test data for image upload:
   ![image](https://github.com/user-attachments/assets/9ae853d1-bdf1-48e2-aced-8a7b762fb96c)
   ![image](https://github.com/user-attachments/assets/dc9a68e4-38e6-4ed4-a8b9-3db541cc85a9)

   Test data for video:
   ![image](https://github.com/user-attachments/assets/0ad9a905-0360-435d-a50c-d73333ec6de9)
   ![image](https://github.com/user-attachments/assets/99f8ea94-764e-4220-9d06-6d18d052c6a7)

   Test data for documents:
   ![image](https://github.com/user-attachments/assets/c9b2349e-945b-4d1d-9d2c-fdf72d09948b)
   ![image](https://github.com/user-attachments/assets/750e28b6-0e64-46d7-956b-cc70328ca582)
   When the link is clicked:
   ![image](https://github.com/user-attachments/assets/7a9acc6b-a5b2-4032-b0b4-18bbeaeaa3b8)

7. **In-app chat messaging function**: On the left side of the window, the submitted reports will display and allow the following message:
   - **1/help**: will display a support message
     ![image](https://github.com/user-attachments/assets/777d328e-7c8f-47e7-be8a-9cebb8da5fe5)

   - **2/view**: will display all the submitted reports
     ![image](https://github.com/user-attachments/assets/cc682d73-25bc-4a45-af5c-61a9cf5fd5ec)
     ![image](https://github.com/user-attachments/assets/ada94180-3d8f-40a1-bed8-6c2b07936f44)
     ![image](https://github.com/user-attachments/assets/a0ab9715-e1a1-4e33-b685-1ffebcd131a4)
   
   - **3/new**: will start a new message
     ![image](https://github.com/user-attachments/assets/71acafbe-c2ff-4b48-84d6-63c4959ff04a)

   - **4/search**: will display the report with the same location and category entered
     ![image](https://github.com/user-attachments/assets/0cd1a3bf-30d4-491f-8d40-da69e547721f)
     ![image](https://github.com/user-attachments/assets/d567f7cd-fd0a-4994-ac99-46fe8342eb99)
     ![image](https://github.com/user-attachments/assets/4e9f2298-fd11-4b9a-95b5-89f535b0e73e)
     ![image](https://github.com/user-attachments/assets/09d97ab0-a87f-4e2c-87f5-907eb6aaa1e9)

   - **Progress Bar Completed**: When the progress bar is completed, it will trun green and display a message to the user.
     ![image](https://github.com/user-attachments/assets/8fb5c475-b59f-4b93-9096-3793308689f7)
     ![image](https://github.com/user-attachments/assets/20b37450-f3d8-401c-a4d3-9cb520db874c)
 
### Key Functions
- **Form Navigation**: Use navigation buttons (e.g., "Back to Main Menu") to move between different sections of the application.
- **Data Handling**: All reported issues will be stored in a local list, using appropriate data structures for efficient management.
- **User engagement**: Chat function and progress bar with motivational lables. 

## Design 
- **User-Centric Design**: The application is designed to be intuitive and easy to use, with clear labels and instructions.
- **Consistency**: A consistent colour scheme and layout are maintained throughout the application.
- **Responsiveness**: The interface is designed to work well across different screen sizes.

