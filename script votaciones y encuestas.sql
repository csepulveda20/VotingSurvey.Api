CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

-- ==========================================

-- TABLAS MAESTRAS

-- ==========================================



CREATE TABLE question_types (

    id VARCHAR(10) PRIMARY KEY,

    name VARCHAR(50) NOT NULL

);



CREATE TABLE tie_rules (

    id VARCHAR(10) PRIMARY KEY,

    name VARCHAR(50) NOT NULL

);



CREATE TABLE quorum_method (

    id VARCHAR(50) PRIMARY KEY,

    name VARCHAR(255) NOT NULL

);



CREATE TABLE assembly_state (

    id VARCHAR(50) PRIMARY KEY,

    name VARCHAR(255) NOT NULL

);



CREATE TABLE power_status (

    id VARCHAR(50) PRIMARY KEY,

    name VARCHAR(255) NOT NULL

);



-- ==========================================

-- ASAMBLEAS

-- ==========================================



CREATE TABLE assembly (

    id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),

    name VARCHAR(255) NOT NULL,

    organized_by UUID NOT NULL,

    date TIMESTAMPTZ NOT NULL,

    state_id VARCHAR(50) REFERENCES assembly_state(id),

    description VARCHAR(255),

    total_quorum INTEGER,

    quorum_method VARCHAR(50) REFERENCES quorum_method(id),

    create_at TIMESTAMPTZ DEFAULT CURRENT_TIMESTAMP,

    update_at TIMESTAMPTZ

);



CREATE TABLE allowed_users_assembly (

    id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),

    name VARCHAR(50) NOT NULL,

    assistance_status BOOLEAN DEFAULT FALSE,

    is_allowed_vote BOOLEAN DEFAULT TRUE,

    user_id UUID NOT NULL REFERENCES groups_place_roles_users(id),

    assembly_id UUID NOT NULL REFERENCES assembly(id)

);



CREATE TABLE power (

    id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),

    file_url TEXT,

    acept_at TIMESTAMPTZ,

    reject_at TIMESTAMPTZ,

    created_at TIMESTAMPTZ DEFAULT CURRENT_TIMESTAMP,

    

    assembly_id UUID NOT NULL REFERENCES assembly(id),

    

    request_by_id UUID REFERENCES allowed_users_assembly(id),

    received_by_id UUID REFERENCES allowed_users_assembly(id),

    autorized_by_id UUID REFERENCES allowed_users_assembly(id),

    

    reject_by_id BOOLEAN DEFAULT FALSE,

    razon_reject VARCHAR(250),

    power_status_id VARCHAR(50) REFERENCES power_status(id)

);



-- ==========================================

-- VOTACIONES Y ENCUESTAS

-- ==========================================



CREATE TABLE questions (
    id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    assembly_id UUID NULL REFERENCES assembly(id) ,
    created_by_id UUID NOT null references groups_place_roles_users(id),
    parent_id UUID NULL REFERENCES questions(id),
    tie_rule_id VARCHAR(10) NOT NULL REFERENCES tie_rules(id),
    question_type_id VARCHAR(10) REFERENCES question_types(id),
    title VARCHAR(100) NOT NULL,
    description VARCHAR(255) NOT NULL,
    is_anonymous BOOLEAN DEFAULT FALSE,
    has_coefficient BOOLEAN DEFAULT FALSE,
    total_votes INT NULL,
    current_total_votes INT NULL,
    limit_options INT NULL,
    score_min INT NULL,
    score_max INT NULL,
    score_style VARCHAR(10) NULL,
    created_at TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    updated_at TIMESTAMPTZ
);



CREATE TABLE questions_files (
    id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    question_id UUID NOT NULL REFERENCES questions(id),
    name VARCHAR(255) NOT NULL,
    description VARCHAR(255),
    url TEXT NOT NULL,
    type VARCHAR(30),
    created_at TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    update_at TIMESTAMPTZ

);



CREATE TABLE allowed_users (
	id UUID primary key default uuid_generate_v4()
    user_id UUID NOT null references groups_place_roles_users(id),
    question_id UUID NOT NULL REFERENCES questions(id),
    unique(user_id, question_id)
);



CREATE TABLE options (
    id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    question_id UUID NOT NULL REFERENCES questions(id),
    value VARCHAR(255) NOT NULL,
    total_count INT DEFAULT 0,
    activity_percentage FLOAT DEFAULT 0,
    created_at TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    updated_at TIMESTAMPTZ
);



CREATE TABLE votes (
    option_id UUID NOT NULL REFERENCES options(id),
    allowed_user_id UUID NOT NULL REFERENCES allowed_users(id),
    quality_vote BOOLEAN DEFAULT FALSE,    
    created_at TIMESTAMPTZ NOT NULL DEFAULT NOW(),    
    PRIMARY KEY (option_id, allowed_user_id)
);



-- Índices Asamblea

CREATE INDEX idx_assembly_state ON assembly(state_id);

CREATE INDEX idx_assembly_quorum ON assembly(quorum_method);

CREATE INDEX idx_allowed_users_assembly_asm ON allowed_users_assembly(assembly_id);

CREATE INDEX idx_power_assembly ON power(assembly_id);

CREATE INDEX idx_power_status ON power(power_status_id);



-- Índices Votaciones

CREATE INDEX idx_questions_assembly ON questions(assembly_id);

CREATE INDEX idx_questions_parent ON questions(parent_id);

CREATE INDEX idx_questions_tie_rule ON questions(tie_rule_id);

CREATE INDEX idx_questions_files_question ON questions_files(question_id);

CREATE INDEX idx_options_question ON options(question_id);

CREATE INDEX idx_allowed_users_question ON allowed_users(question_id);

CREATE INDEX idx_votes_option ON votes(option_id);