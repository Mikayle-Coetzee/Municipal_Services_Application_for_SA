# ST10023767_PROG7312 (Municipal Services Application for South Africa)

This project is part of a POE where I developed a **WPF Application** using **C#** and the **.NET Framework**. The application is designed for managing municipal services in South Africa, offering features that allow users to report issues, request services, and view local events and announcements. The goal is to create an efficient and user-friendly platform that enhances the interaction between citizens and municipal services.

## Author 
#### Mikayle Devpnique Coetzee
##### ST10023767

---

## Project Overview
### Part 1:
In the first part, the **"Report Issues"** functionality was implemented. This allows users to submit detailed reports about municipal issues such as sanitation, roads, utilities, etc. For more details on Part 1, see the `README_Part_1.md`.

### Part 2:
This part extends the application by introducing the **Local Events and Announcements** feature and enhancing data structures for efficient performance. Additionally, I implemented an advanced **Recommendation Feature** based on user searches, and I made use of data structures such as **Stacks**, **Priority Queues**, **Dictionaries**, **Sorted Dictionaries**, and **Sets**.

---

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

---

## Usage Instructions

### Main Menu
After launching the application, the main menu provides three options:
- **Report Issues** (Active)
- **Local Events and Announcements** (Active)
- **Service Request Status** (Disabled, coming soon)
![image](https://github.com/user-attachments/assets/7f2952f4-05c4-45e9-b7de-c25af1269140)

### Reporting an Issue (Part 1; README_Part_1.md)
To report a municipal issue, navigate to the **Report Issues** section and fill in the required details inorder to submit the report.

### Local Events and Announcements
![image](https://github.com/user-attachments/assets/e0735985-ea1f-4033-91e0-a67969f3731e)

1. **Create Post**: 
   - Click **Create Post** to add an event or announcement.
   - ![image](https://github.com/user-attachments/assets/28427fe2-c806-4532-b214-ac2287f7479a)
   - Provide details such as title, start/end dates, start/end times, venue, and description.
   - Optionally, attach media files.
   - ![image](https://github.com/user-attachments/assets/b5781f7d-6850-4687-9aca-b90736c0220e)
   - ![image](https://github.com/user-attachments/assets/7dc18814-dd34-44eb-bf97-e88802de28a1)
   - ![image](https://github.com/user-attachments/assets/1e967da7-002a-4e63-8c56-7ba69801d0d3)

   - Posts are displayed in a list with color-coded status (red for past, yellow for ongoing, green for upcoming).
   - ![image](https://github.com/user-attachments/assets/b059740a-7465-44b0-965c-25bcb6b2bfe0)
   - ![image](https://github.com/user-attachments/assets/cff28f03-dc20-4070-bda3-ba8669ca6e70)
   - ![image](https://github.com/user-attachments/assets/fe8570ba-816c-4a95-a68e-a48c57318aba)
   - ![image](https://github.com/user-attachments/assets/b26d36f6-9d91-495a-824d-2ec09e3895d7)
   - ![image](https://github.com/user-attachments/assets/e262badf-229b-4f81-b1f3-dd398723905a)
   - ![image](https://github.com/user-attachments/assets/64ec7911-cf95-42a5-b945-a33ae7adda58)
   - ![image](https://github.com/user-attachments/assets/01c6374e-732f-4313-a021-f5c2aecb98f6)
   - ![image](https://github.com/user-attachments/assets/240d08b4-eaea-44f8-87dc-6cf4195a9223)

2. **Filtering and Sorting**:
   - Use filters at the top of the page to sort by **Events**, **Announcements**, **Category**, **Upcoming**, **Busy**, **Past**.
   - ![image](https://github.com/user-attachments/assets/a92bbbbc-3bda-40b0-bd99-c8b2825e32a4)
   - ![image](https://github.com/user-attachments/assets/87be43fb-85a4-4187-8ee0-9e9ad2e5877d)

   - Filter categories based on existing posts.
   - Select "All Areas" or filter by specific regions.

3. **Recommendations**:
   - Search for events, and based on your search history, personalized recommendations will appear in the **Recommended** section.
   - ![image](https://github.com/user-attachments/assets/d3e60867-3faf-4e5f-9384-20632cd68788)
   - ![image](https://github.com/user-attachments/assets/4ad23ab5-1f6a-428a-a2e1-3906b4960850)
   - ![image](https://github.com/user-attachments/assets/1d0cee71-99dc-4223-8869-ebcad30c67cd)
   - ![image](https://github.com/user-attachments/assets/3c9f2adc-1827-47b6-8b3e-4e994ad6787e)

### Navigation
- Use the navigation buttons (e.g., **Back to Main Menu**) to switch between sections of the application.

---

## Features
### **Report Issues**
- Users can report municipal issues by specifying details like:
  - Location 
  - Issue Category (e.g., Sanitation, Roads, Utilities)
  - Description of the issue
  - Attach media (images/videos)
  
### **Local Events and Announcements**
- Users can create posts for events and announcements.
- Categories include Music, Art, Sports, etc.
- Posts can be filtered by categories and sorted by dates.
- User-friendly form for adding:
  - Event title
  - Start and end dates
  - Start and end times
  - Venue location
  - Detailed descriptions
  - Media attachments (optional)
- Personalized recommendations based on user search history are displayed.

### **Service Request Status** (Coming Soon)
- The service request feature will be implemented in a future phase.

### **Recommendation Feature**
- Analyzes user search patterns.
- Suggests events and announcements based on preferences using advanced algorithms.
- Recommendations are ordered by priority (weight/score).

---

## Data Structures Used
- **Stacks, Queues, and Priority Queues**: Efficient management of event data for processing and prioritizing recommendations.
- **Dictionaries and Sorted Dictionaries**: Organizing and retrieving events by categories, dates, and other filters.
- **Sets**: Handling unique categories and filtering for user queries.

---

## Design Principles
- **User-Centric Design**: The application is designed for ease of use with intuitive navigation and clear labels.
- **Consistency**: The UI follows a consistent color scheme and layout for an enhanced user experience.
- **Responsiveness**: The interface adapts to different screen sizes for optimal viewing.

---

## Future Enhancements
- **Service Request Status** will be implemented in the next part to allow users to track their service requests.
- Further improvements to the recommendation system will be added when multiple users can use the app and data can be stored in a database.

---

## License
This project is developed as part of an educational assignment and is for learning purposes only. But MIT License was selected.

---
