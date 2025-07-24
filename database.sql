CREATE TABLE companies (
	id INT PRIMARY KEY AUTO_INCREMENT,
	company_name VARCHAR(200) NOT NULL,
	created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
);

CREATE TABLE departments (
	id INT PRIMARY KEY AUTO_INCREMENT,
    company_id INT NOT NULL,
	department_name VARCHAR(200) NOT NULL,
	created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
	FOREIGN KEY (company_id) REFERENCES companies(id)
);

CREATE TABLE divisions (
	id INT PRIMARY KEY AUTO_INCREMENT,
	company_id INT NOT NULL,
	department_id INT,
	division_name VARCHAR(200) NOT NULL,
	created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
	FOREIGN KEY (company_id) REFERENCES companies(id),
	FOREIGN KEY (department_id) REFERENCES departments(id)
);

CREATE TABLE employees (
	id INT PRIMARY KEY AUTO_INCREMENT,
	employee_name VARCHAR(200) NOT NULL,
	created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
);

CREATE TABLE licenses (
	id INT PRIMARY KEY AUTO_INCREMENT,
	license_name VARCHAR(200) NOT NULL,
	created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
);

CREATE TABLE department_assignments (
	employee_id INT NOT NULL,
    department_id INT NOT NULL,
	PRIMARY KEY (employee_id, department_id),
    FOREIGN KEY (employee_id) REFERENCES employees(id) ON DELETE CASCADE,
    FOREIGN KEY (department_id) REFERENCES departments(id) ON DELETE CASCADE
);

CREATE TABLE division_assignments (
	employee_id INT NOT NULL,
    division_id INT NOT NULL,
	PRIMARY KEY (employee_id, division_id),
    FOREIGN KEY (employee_id) REFERENCES employees(id) ON DELETE CASCADE,
    FOREIGN KEY (division_id) REFERENCES divisions(id) ON DELETE CASCADE
);

CREATE TABLE holdings (
    employee_id INT NOT NULL,
	license_id INT NOT NULL,
	PRIMARY KEY (employee_id, license_id),
    FOREIGN KEY (employee_id) REFERENCES employees(id) ON DELETE CASCADE,
    FOREIGN KEY (license_id) REFERENCES licenses(id) ON DELETE CASCADE
);