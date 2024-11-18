# ST10023767_PROG7312 (Municipal Services Application for South Africa)

This project is part of a POE where I developed a **WPF Application** using **C#** and the **.NET Framework**. The application is designed for managing municipal services in South Africa, offering features that allow users to report issues, request services, view local events and announcements, and track service request statuses. The goal is to create an efficient and user-friendly platform that enhances the interaction between citizens and municipal services.

## Author 
#### Mikayle Devpnique Coetzee
##### ST10023767

---

## Project Overview
### Part 1:
In the first part, the **"Report Issues"** functionality was implemented. This allows users to submit detailed reports about municipal issues such as sanitation, roads, utilities, etc. For more details on Part 1, see the `README_Part_1.md`.

### Part 2:
This part extends the application by introducing the **Local Events and Announcements** feature and enhancing data structures for efficient performance. Additionally, I implemented an advanced **Recommendation Feature** based on user searches, and I made use of data structures such as **Stacks**, **Priority Queues**, **Dictionaries**, **Sorted Dictionaries**, and **Sets**.

### Part 3:
This part extends the application by introducing the **Service Request Status** feature and enhancing data structures for efficient performance. Additionally, I implemented a **Status Tracking Feature** and **Filtering Feature**.I made use of data structures such as **Basic Trees**, **Binary Trees**, **Binary Search Trees**, **AVL Trees**, **Red-Black Trees**, **Heaps**, **Graphs and Graph Traversal** and a **Minimum Spanning Tree**.

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
- **Service Request Status** (Active)
![image](https://github.com/user-attachments/assets/7f2952f4-05c4-45e9-b7de-c25af1269140)

### Reporting an Issue (Part 1; README_Part_1.md)
To report a municipal issue, navigate to the **Report Issues** section and fill in the required details inorder to submit the report.

### Posting an Event (Part 2; README_Part_2.md)
To post an event, navigate to the **Local Events and Announcements** section and fill in the required details inorder to post an event or announcement.

### Service Request Status List
![image](https://github.com/user-attachments/assets/e0735985-ea1f-4033-91e0-a67969f3731e)

1. **Create Post**: 
   - Click **Create Post** to add an event or announcement.

2. **Filtering and Sorting**:
   - Use filters at the top of the page to sort by **All**, **Date**, **Category**, **Pending**, **Active**, **Resolved**.
   - Filter categories based on existing service issues.

### Service Request Status Tracking
![image](https://github.com/user-attachments/assets/e0735985-ea1f-4033-91e0-a67969f3731e)

1. **Tracking All Submitted Service Issues**: 
   - Click **Track Service Issue**

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
- 
### **Recommendation Feature**
- Analyzes user search patterns.
- Suggests events and announcements based on preferences using advanced algorithms.
- Recommendations are ordered by priority (weight/score).

### **Service Request Status** 
- Users can view a list of all submitted service requests.
- Users can filter through the srevice requests
- Users can search for a specific issue submitted
- Users can view a dashboard of graphs and a data grid to track services reported

---

## Data Structures Used in Part 2
- **Stacks, Queues, and Priority Queues**: Efficient management of event data for processing and prioritizing recommendations.
- **Dictionaries and Sorted Dictionaries**: Organizing and retrieving events by categories, dates, and other filters.
- **Sets**: Handling unique categories and filtering for user queries.
  
## Data Structures Used in Part 3
- **Basic Trees, Binary Trees, Binary Search Trees (BST), AVL Trees, Red-Black Trees, Heaps, Graphs, Graph Traversal, and Minimum Spanning Tree**: For more detailes, please read the **Implementation_Report.pdf**.

---

## Reports and documents for Part 3 found within the folder thats submitted
- **Implementation_Report.pdf**
- **Project_Completion_Report.pdf**
- **Technology_Recommendations_Report.pdf**
- **Updates_Based_on_Feedback.pdf**

## Design Principles
- **User-Centric Design**: The application is designed for ease of use with intuitive navigation and clear labels.
- **Consistency**: The UI follows a consistent color scheme and layout for an enhanced user experience.
- **Responsiveness**: The interface adapts to different screen sizes for optimal viewing.

---

## Future Enhancements
- Further improvements to the recommendation system will be added when multiple users can use the app and data can be stored in a database.

---

## License
This project is developed as part of an educational assignment and is for learning purposes only. But MIT License was selected.

---
