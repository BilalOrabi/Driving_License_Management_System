# DVLD Project – Driving License Management System

## Overview  
The Driving & Vehicle License Department (DVLD) system is designed to manage the issuance and administration of driving licenses. It encompasses a range of services to ensure the provision of safe drivers on the roads.

## Installation  
1. Locate the "DVLD.bak" file in the "Database" folder and restore it to your preferred DBMS.  
2. Download the project from the repository.  
3. Extract the downloaded files to your computer and Run the Project.  
4. Open the solution and right-click on the DVLD_Presentation_Layer project, then click "set as startup".  
5. Once the project is running, the login screen will appear.  
6. Enter the username as 'admin' and the password as '1234' to log in.

## Technical Details  
- **Framework**: WindForm (built on .Net 8)  
- **Programming Language**: C#  
- **Data Access**: ADO.NET
- **Architecture**: The system follows a **3-Tier Architecture**:  
  1. **Presentation Layer**: Handles the user interface (UI) and interactions with the system.  
  2. **Business Logic Layer**: Contains the business logic and rules governing the application.  
  3. **Data Access Layer**: Responsible for interacting with the database and ensuring efficient data retrieval and manipulation.


## Services Offered  
1. **Issuance of First-Time License**: A service for applicants to obtain a driving license for the first time.  
2. **Re-examination Service**: Allows applicants who have failed a test to schedule a re-examination.  
3. **License Renewal Service**: For renewing existing driving licenses.  
4. **Duplicate License for Lost License**: Issuance of a replacement for a lost driving license.  
5. **Duplicate License for Damaged License**: Issuance of a replacement for a damaged driving license.  
6. **License Release Service**: Unblocking a driving license after paying the required fines.  
7. **International License Issuance**: Providing an international driving license to eligible applicants.

## Application Information  
Applicants must submit a request for the desired service and pay the associated fees. The system will require the following information:  
- Application number  
- Application date  
- Applicant's personal number (national ID)  
- Type of service requested  
- Application status (new, canceled, completed)  
- Paid fees

## License Categories  
The system offers various license categories, each with specific requirements:  
- **Category 1**: Small Motorcycles  
- **Category 2**: Heavy Motorcycles  
- **Category 3**: Regular Driving (Cars)  
- **Category 4**: Commercial Driving (Taxi/Limousine)  
- **Category 5**: Agricultural Vehicles  
- **Category 6**: Small and Medium Buses  
- **Category 7**: Trucks and Heavy Vehicles

## System Management  
The system provides functionalities for managing:  
- Users  
- Individuals  
- Requests  
- Tests  
- License categories  
- License reservations

## Requirements for License Application  
Applicants must meet the age requirement for the desired category, not possess a license of the same category, provide valid personal documents, and complete the necessary education and training.

## Testing Process  
Applicants must pass a series of tests in the following order:  
1. Vision Test  
2. Theoretical Test  
3. Practical Driving Test  

Upon passing all tests and meeting the conditions, a driving license is issued with a validity period as defined in the license category.

## Inquiry Service  
The system allows for inquiries about licenses held by an individual using their national number or license number.

## Conclusion  
The DVLD system streamlines the process of managing driving licenses, ensuring that all drivers meet the necessary safety standards.
