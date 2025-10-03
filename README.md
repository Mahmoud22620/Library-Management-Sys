# Library Management System - Design Overview

A modern Library Management System built with **ASP.NET Core Web API** and **.NET 9**, featuring comprehensive book management, user authentication, and role-based access control.

## Core Architecture

- **Controllers**: Handle HTTP requests and responses
- **Services**: Contain business logic and domain rules
- **Repositories**: Manage data access with generic repository pattern
- **Models**: Domain entities and database structure
- **DTOs**: API data contracts separate from domain models

## Security & Authentication

**JWT Bearer Authentication** chosen for:
- Stateless operation (no server sessions)
- Cross-platform compatibility
- Scalable for distributed systems

**Fine-Grained Permissions System**:
- Bit-flag based permissions for granular control
- Beyond simple roles (Admin, Librarian, Staff)
- Each action (Create, Read, Update, Delete) controlled separately
- Example: `Books_Create`, `Members_View`, `Authors_Delete`

**Admin Permission Control**:
- **Complete User Management**: Admin can independently control user permissions
- **Role Assignment**: Admin can assign/change roles for any user account
- **Dynamic Permission Updates**: Real-time permission changes without system restart
- **Centralized Control**: Single admin dashboard for all user permission management
- **API Endpoint**: `POST /api/Auth/AssignRole` - Admin-only access for role management

**ASP.NET Core Identity Integration**:
- Built-in password hashing and security
- Extensible user model with custom permissions
- Role-based authentication with GUID primary keys

## Database Design

**Entity Framework Core with Code-First**:
- Migration-based schema versioning
- Strongly-typed database access
- Cross-platform database support

**Key Relationship Patterns**:
- **Many-to-Many**: Books ↔ Authors (via BookAuthor junction table)
- **Hierarchical**: Categories with parent-child relationships
- **One-to-Many**: Publishers → Books, Categories → Books
- **Cascade Protection**: `DeleteBehavior.Restrict` prevents accidental data loss

**Comprehensive Seed Data**:
- Pre-loaded with 15 books, 15 authors, 10 publishers
- Hierarchical categories (Fiction → Fantasy, Horror, etc.)
- Three user roles with appropriate permissions
- Ready-to-use system after deployment

## Data Management

**AutoMapper Integration**:
- Automatic object-to-object mapping
- Separate DTOs for read/write operations
- Custom mappings for complex scenarios (e.g., flattening author names)

**Repository Pattern Benefits**:
- Generic repository for consistent CRUD operations
- Specialized repositories for complex business logic
- Consistent data access patterns

## Background Services

**Automated Business Rules**:
- Daily background service checks for overdue books
- Automatically updates transaction status
- Reduces manual administrative work
- Ensures data consistency

## File Management

**Book Cover Images**:
- Stored in `wwwroot/images/Books/`
- Automatic file naming and version control
- Cache-busting for updated images
- Proper cleanup when books are deleted
- Multipart form data support for uploads

## API Design

**RESTful Principles**:
- Standard HTTP verbs and status codes
- Consistent response formats
- Proper error handling with meaningful messages
- Swagger/OpenAPI documentation with JWT support

## Audit & Monitoring

**Complete Activity Logging**:
- Every user action recorded with timestamps
- Permission-based action tracking
- User identification and audit trails
- Security monitoring and compliance support

## Admin Capabilities

**Comprehensive User Control**:
- **Independent Permission Management**: Admin can control user accounts without IT intervention
- **Role-Based Assignment**: Assign users as Admin, Librarian, or Staff
- **Real-Time Updates**: Permission changes take effect immediately
- **Secure Access**: Admin-only endpoints protected by `[Authorize(Roles = "Admin")]`
- **Complete User Lifecycle**: Create, modify, and manage user permissions

**Permission Hierarchy**:
- **Admin**: Full system access (all CRUD operations on all entities)
- **Librarian**: Book and transaction management, member viewing/creation
- **Staff**: Member management, transaction handling, read-only access to library data

## Key Benefits

1. **Scalable**: Stateless JWT authentication, generic repositories
2. **Secure**: Fine-grained permissions, comprehensive authentication
3. **Maintainable**: separation of concerns
4. **Extensible**: Easy to add new entities, permissions, and features
5. **Production-Ready**: Error handling, logging, background services
6. **Developer-Friendly**: Swagger docs, AutoMapper, async/await patterns
7. **Admin-Controlled**: Self-service user management without technical intervention

## Technology Stack

- **Backend**: ASP.NET Core Web API (.NET 9)
- **Database**: Entity Framework Core with SQL Server
- **Authentication**: JWT Bearer + ASP.NET Core Identity
- **Documentation**: Swagger/OpenAPI
- **Mapping**: AutoMapper
- **Background Tasks**: Hosted Services

This architecture provides a robust foundation for managing library operations while maintaining security, performance, and complete administrative control over user permissions.
