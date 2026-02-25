// ========================================================
// Shree Om Hardware – Site JS
// ========================================================

// Password show/hide toggle
function togglePw(inputId, btn) {
    var input = document.getElementById(inputId);
    var icon = btn.querySelector('i');
    if (input.type === 'password') {
        input.type = 'text';
        icon.classList.replace('fa-eye', 'fa-eye-slash');
    } else {
        input.type = 'password';
        icon.classList.replace('fa-eye-slash', 'fa-eye');
    }
}

// Auto-hide success alerts after 5 seconds
document.addEventListener('DOMContentLoaded', function () {
    document.querySelectorAll('.alert-success').forEach(function (alert) {
        setTimeout(function () {
            alert.style.transition = 'opacity 0.5s';
            alert.style.opacity = '0';
            setTimeout(function () { alert.style.display = 'none'; }, 500);
        }, 5000);
    });

    // Highlight active nav link
    document.querySelectorAll('.navbar-links a').forEach(function (link) {
        if (link.href === window.location.href) link.classList.add('active');
    });

    // ── Real-time Login Validation ────────────────────────────
    const loginForm = document.getElementById('loginForm');
    if (loginForm) {
        const emailInput = document.getElementById('loginEmail');
        const passInput = document.getElementById('loginPassword');

        function validateLoginEmail() {
            const val = emailInput.value.trim();
            const errEl = document.getElementById('loginEmailErr');
            if (!val) {
                showFieldError(emailInput, errEl, 'Email is required');
                return false;
            }
            if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(val)) {
                showFieldError(emailInput, errEl, 'Please enter a valid email address');
                return false;
            }
            clearFieldError(emailInput, errEl);
            return true;
        }

        function validateLoginPass() {
            const val = passInput.value;
            const errEl = document.getElementById('loginPassErr');
            if (!val) {
                showFieldError(passInput, errEl, 'Password is required');
                return false;
            }
            clearFieldError(passInput, errEl);
            return true;
        }

        emailInput?.addEventListener('blur', validateLoginEmail);
        emailInput?.addEventListener('input', validateLoginEmail);
        passInput?.addEventListener('blur', validateLoginPass);
        passInput?.addEventListener('input', validateLoginPass);

        loginForm.addEventListener('submit', function (e) {
            const validEmail = validateLoginEmail();
            const validPass = validateLoginPass();
            if (!validEmail || !validPass) e.preventDefault();
        });
    }

    // ── Real-time Register Validation ─────────────────────────
    const regForm = document.getElementById('registerForm');
    if (regForm) {
        const rules = [
            {
                id: 'regFullName', errId: 'regFullNameErr',
                validate: v => !v.trim() ? 'Full name is required' :
                    v.trim().length < 2 ? 'Name must be at least 2 characters' : ''
            },
            {
                id: 'regEmail', errId: 'regEmailErr',
                validate: v => !v.trim() ? 'Email is required' :
                    !/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(v) ? 'Please enter a valid email address' : ''
            },
            {
                id: 'regPhone', errId: 'regPhoneErr',
                validate: v => !v.trim() ? 'Phone number is required' :
                    !/^[+\d][\d\s\-]{8,}$/.test(v.trim()) ? 'Please enter a valid phone number' : ''
            },
            {
                id: 'regPassword', errId: 'regPasswordErr',
                validate: v => !v ? 'Password is required' :
                    v.length < 6 ? 'Password must be at least 6 characters' :
                        !/[A-Z]/.test(v) && !/[0-9]/.test(v) ? 'Password should contain letters and numbers' : ''
            },
            {
                id: 'regConfirmPassword', errId: 'regConfirmPasswordErr',
                validate: v => {
                    const pw = document.getElementById('regPassword')?.value || '';
                    return !v ? 'Please confirm your password' :
                        v !== pw ? 'Passwords do not match' : '';
                }
            }
        ];

        rules.forEach(rule => {
            const input = document.getElementById(rule.id);
            const errEl = document.getElementById(rule.errId);
            if (!input || !errEl) return;
            function validate() {
                const msg = rule.validate(input.value);
                if (msg) showFieldError(input, errEl, msg);
                else clearFieldError(input, errEl);
                return !msg;
            }
            input.addEventListener('blur', validate);
            input.addEventListener('input', validate);
        });

        // Password strength indicator
        const pwInput = document.getElementById('regPassword');
        const strengthBar = document.getElementById('pwStrengthBar');
        pwInput?.addEventListener('input', function () {
            if (!strengthBar) return;
            const v = this.value;
            let score = 0;
            if (v.length >= 6) score++;
            if (v.length >= 10) score++;
            if (/[A-Z]/.test(v)) score++;
            if (/[0-9]/.test(v)) score++;
            if (/[^A-Za-z0-9]/.test(v)) score++;
            const colors = ['#EF4444', '#F97316', '#EAB308', '#22C55E', '#16A34A'];
            const labels = ['Very Weak', 'Weak', 'Fair', 'Good', 'Strong'];
            strengthBar.style.width = (score * 20) + '%';
            strengthBar.style.background = colors[score - 1] || '#E5E7EB';
            const lbl = document.getElementById('pwStrengthLabel');
            if (lbl) lbl.textContent = score > 0 ? labels[score - 1] : '';
        });

        regForm.addEventListener('submit', function (e) {
            let valid = true;
            rules.forEach(rule => {
                const input = document.getElementById(rule.id);
                const errEl = document.getElementById(rule.errId);
                const msg = rule.validate(input?.value || '');
                if (msg) { showFieldError(input, errEl, msg); valid = false; }
                else { clearFieldError(input, errEl); }
            });
            const terms = document.getElementById('regTerms');
            const termsErr = document.getElementById('regTermsErr');
            if (terms && !terms.checked) {
                if (termsErr) termsErr.textContent = 'You must agree to the Terms & Conditions';
                valid = false;
            } else if (termsErr) {
                termsErr.textContent = '';
            }
            if (!valid) e.preventDefault();
        });
    }

    // ── Helpers ────────────────────────────────────────────────
    function showFieldError(input, errEl, msg) {
        if (input) input.style.borderColor = '#EF4444';
        if (errEl) errEl.textContent = msg;
    }
    function clearFieldError(input, errEl) {
        if (input) input.style.borderColor = '';
        if (errEl) errEl.textContent = '';
    }
});
